#!/bin/bash

# Step 1: Check if the token is provided as a command-line argument
if [ -z "$1" ]; then
  echo "Error: SonarQube token is required as a command-line argument."
  echo "Usage: ./run_sonarqube.sh <sonar-token>"
  sleep 5
  exit 1
fi

SONAR_TOKEN=$1

# Check if installation was successful
if [ $? -ne 0 ]; then
  echo "Error: Failed to install dotnet-sonarscanner."
  sleep 10
  exit 1
fi

# Step 3: Start SonarQube analysis
echo "Starting SonarQube analysis..."
dotnet sonarscanner begin -k:"Predictive-Healthcare-Management-System" -d:sonar.token="$SONAR_TOKEN" -d:sonar.cs.vscoveragexml.reportsPaths=coverage.xml -d:sonar.host.url="http://localhost:9000" 

# Check if the SonarQube analysis start was successful
if [ $? -ne 0 ]; then
  echo "Error: Failed to start SonarQube analysis."
  pwd
  sleep 1000
  exit 1
fi

# Step 4: Build the project
echo "Building the project..."
dotnet build --no-incremental

# Check if the build was successful
if [ $? -ne 0 ]; then
  echo "Error: Build failed."
  sleep 10
  exit 1
fi

# Step 4.1: Collect code coverage
echo "Collecting code coverage..."
dotnet-coverage collect "dotnet test" -f xml -o "coverage.xml"

# Check if code coverage collection was successful
if [ $? -ne 0 ]; then
  echo "Error: Code coverage collection failed."
  sleep 10
  exit 1
fi

# Step 5: End SonarQube analysis
echo "Ending SonarQube analysis..."
dotnet sonarscanner end -d:sonar.token="$SONAR_TOKEN"

# Check if the SonarQube analysis end was successful
if [ $? -ne 0 ]; then
  echo "Error: Failed to complete SonarQube analysis."
  sleep 10
  exit 1
fi

echo "SonarQube analysis completed"

sleep 10
