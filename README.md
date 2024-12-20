# Repository Template

[![CI/CD Pull Request](https://github.com/nhs-england-tools/repository-template/actions/workflows/cicd-1-pull-request.yaml/badge.svg)](https://github.com/nhs-england-tools/repository-template/actions/workflows/cicd-1-pull-request.yaml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=repository-template&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=repository-template)

Welcome to the Pathway Coordinator product. The pathway coordinator is the central cog within the National Screen Platform and is responsible for managing a Participant through a Screening Episode.

It is essentially a workflow engine which executes azure functions in a designated order, depending on the screening pathway

## Table of Contents

- [Repository Template](#repository-template)
  - [Table of Contents](#table-of-contents)
  - [Setup](#setup)
    - [Prerequisites](#prerequisites)
    - [Configuration](#configuration)
  - [Design](#design)
    - [Diagrams](#diagrams)
  - [Contributing](#contributing)
  - [Contacts](#contacts)
  - [Licence](#licence)

## Setup

Clone the repository

```shell
git clone https://github.com/NHSDigital/dtos-pathway-coordinator
cd dtos-pathway-coordinator
```

Running this locally, assumes that there is a ServiceBus and SQL Server running in a local docker container. This can be found in the folder /tests/LocalServiceBus

Within that folder, running the following command will stand up a local servicebus and sqledge container that the application connects to.

```shell
sudo docker compose -f docker-compose.yml up
```

What I have found is that the sqledge container can be flaky, but stopping and starting usually results in it working.

### Prerequisites

The following software packages, or their equivalents, are expected to be installed and configured:

- [Docker](https://www.docker.com/) container runtime or a compatible tool, e.g. [Podman](https://podman.io/),

### Configuration

This is a dotnet project and as such the dotnet restore will result in the application running

## Design

The design here centres around the definition of a Pathway, currently this is represented as a json object

  {
    "Name": "High Risk Breast",
    "Steps": [
      { "Type": "AddParticipantToPathway", "TriggerEvent": "ParticipantEligible", "MessageTemplate": {
        "NhsNumber": "{{nhsNumber}}",
        "Pathway" : "{{pathway}}"
        }
      },
      { "Type": "UpdateParticipantPathwayStatus", "TriggerEvent": "ParticipantAccepted", "MessageTemplate": { "Status": "Invited" } }
    ]
  }

  Each pathway comprises of a series of Steps. A step has a triggering event for a specific pathway and a message template representing the data required by the step to perform it's action. The Type of Step will map to a specific Step within the Steps folder of the PathwayCoordinator.PathwayManager project.

  The idea is that the steps can become reuseable steps that can be reused across multiple pathways.

This programme currently has 9 separate projects :-

- Audit.API - This provides an API that allows the creation of a Audit Record in the SQL Database
- Audit.Service - This is a message handler, listening for the AuditSubscriber subscription on the service bus. It then invokes the Audit.API to create the audit record
- PathwayCoordinator.API - Provides a restful interface that returns the Pathway definitions, currently being pulled from a local json file
- PathwayCoordinator.Interfaces - Collection of interfaces used across all projects, a way of avoiding cyclic dependencies
- PathwayCoordinator.Models - Set of Domain models used across all projects
- PathwayCoordinator.Messaging - Message handler that subscribes the PathwayInvocationSubscription and invokes the PathwayManager
- PathwayCoordinator.PathwayManager - Invoked by the message handler than knows about the state that's been passed, brings together the data needed for the next call and invokes a PathwayStep
- PathwayCoordinator.Tests - Just some simple tests
- PathwayCoordinator.UI - Web interface used to put messages on the queue and test the integration between the components

### Diagrams

For more detailed diagrams see this link - <https://nhsd-confluence.digital.nhs.uk/display/DTS/Pathway+Coordinator>

## Contributing

Describe or link templates on how to raise an issue, feature request or make a contribution to the codebase. Reference the other documentation files, like

- Environment setup for contribution, i.e. `CONTRIBUTING.md`
- Coding standards, branching, linting, practices for development and testing
- Release process, versioning, changelog
- Backlog, board, roadmap, ways of working
- High-level requirements, guiding principles, decision records, etc.

## Contacts

Provide a way to contact the owners of this project. It can be a team, an individual or information on the means of getting in touch via active communication channels, e.g. opening a GitHub discussion, raising an issue, etc.

## Licence

> The [LICENCE.md](./LICENCE.md) file will need to be updated with the correct year and owner

Unless stated otherwise, the codebase is released under the MIT License. This covers both the codebase and any sample code in the documentation.

Any HTML or Markdown documentation is [© Crown Copyright](https://www.nationalarchives.gov.uk/information-management/re-using-public-sector-information/uk-government-licensing-framework/crown-copyright/) and available under the terms of the [Open Government Licence v3.0](https://www.nationalarchives.gov.uk/doc/open-government-licence/version/3/).
