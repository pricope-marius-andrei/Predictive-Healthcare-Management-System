#!/bin/bash

# Check if the token is provided
if [ -z "$1" ]; then
  echo "Usage: $0 <sonar-token>"
  sleep 10
  exit 1
fi

# Define variables
SONAR_TOKEN="$1"

# Start SonarScanner
dotnet sonarscanner begin /k:"Predictive-Healthcare-Management-System" /d:sonar.token="$SONAR_TOKEN" /d:sonar.cs.vscoveragexml.reportsPaths="coverage.xml" /d:sonar.host.url="http://localhost:9000"
if [ $? -ne 0 ]; then
  echo "Failed to start SonarScanner"
  sleep 2
  exit 1
fi

# Build the project
dotnet build --no-incremental
if [ $? -ne 0 ]; then
  echo "Failed to build the project"
  sleep 10
  exit 1
fi

# Collect coverage
dotnet-coverage collect "dotnet test" -f xml -o "coverage.xml"
if [ $? -ne 0 ]; then
  echo "Failed to collect coverage"
  sleep 10
  exit 1
fi

# End SonarScanner
dotnet sonarscanner end /d:sonar.token="$SONAR_TOKEN"
if [ $? -ne 0 ]; then
  echo "Failed to end SonarScanner"
  sleep 10
  exit 1
fi
