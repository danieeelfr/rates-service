# Tech Challenge - API 1 (rate-services-api)

## Solution overview

![overview](Resources/Img/api1.png)

## DevOps flow

![devops](Resources/Img/devops.png)

### Continuous integration pipeline

![devops](Resources/Img/ci.gif)

### Continuous delivery pipeline

![devops](Resources/Img/delivery.gif)

## Quality assurance

### Coverage report

![devcoverageops](Resources/Img/coverage.png)

## Running tests

### Unit and/or Integrated tests

```
dotnet test
```

### With coverage

```
dotnet test \                    
  --configuration Development \
  /p:CollectCoverage=true \
  /p:CoverletOutputFormat=cobertura \
  /p:CoverletOutput=./TestResults/Coverage/
```

### Coverage report

```
dotnet tool run reportgenerator \
  -reports:./TestResults/Coverage/coverage.cobertura.xml \
  -targetdir:./CodeCoverage \
  -reporttypes:HtmlInline_AzurePipelines
```

## Dockering

### Build an docker image locally

```
docker build --pull --rm -f "Dockerfile" -t rate-services-api:latest "."
```

### Running the docker image in a locally container

```
docker run --rm -d  -p 5000:5000/tcp -p 5001:5001/tcp rate-services-api:latest
```

## API documentation with Swagger

```
http://localhost:5000/index.html
```

## Built With

* .NET Core 3.1
* .NET Tools
* C#
* GitHub
* Jwt
* Microsoft Azure
* Swagger
* Ubuntu 18.3
* Visual Studio Code

## What I've learning?

* Azure - Apps for containers
* Azure DevOps - Continuous integration (CI)
* Azure DevOps - Continuous delivery (CD)
* Azure DevOps - Quality analisys & coverage reports
* Developing .NET applications using Ubuntu and vscode.
* Docker - dockering .NET applications
* Docker - Azure Container Registry
* GitHub actions (not applied)
* Integration tests
* Pact tests (not applied)
* Unit tests with xUnity
* Whimsical (draws and diagrams)
