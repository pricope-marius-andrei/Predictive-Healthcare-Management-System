#!/bin/bash

# Step 1: Check if the token is provided as a command-line argument
if [ -z "$1" ]; then
  echo "Error: SonarQube token is required as a command-line argument."
  echo "Usage: ./run_sonarqube.sh <sonar-token>"
  sleep 5
  exit 1
fi

SONAR_TOKEN=$1

# Step 2: Install dotnet-sonarscanner tool globally
echo "Installing dotnet-sonarscanner..."
dotnet tool install --global dotnet-sonarscanner

# Check if installation was successful
if [ $? -ne 0 ]; then
  echo "Error: Failed to install dotnet-sonarscanner."
  sleep 10
  exit 1
fi

# Step 3: Start SonarQube analysis
echo "Starting SonarQube analysis..."
dotnet sonarscanner begin -k:"Predictive-Healthcare-Management-System" -d:sonar.host.url="http://localhost:9000" -d:sonar.token="$SONAR_TOKEN"

# Check if the SonarQube analysis start was successful
if [ $? -ne 0 ]; then
  echo "Error: Failed to start SonarQube analysis."
  pwd
  sleep 1000
  exit 1
fi

# Step 4: Build the project
echo "Building the project..."
dotnet build

# Check if the build was successful
if [ $? -ne 0 ]; then
  echo "Error: Build failed."
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
