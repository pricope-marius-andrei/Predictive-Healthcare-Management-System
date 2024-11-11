#!/bin/bash

# Step 1: Install dotnet-sonarscanner tool globally
echo "Installing dotnet-sonarscanner..."
dotnet tool install --global dotnet-sonarscanner

# Step 2: Start SonarQube analysis
echo "Starting SonarQube analysis..."
dotnet sonarscanner begin /k:"Predictive-Healthcare-Management-System" /d:sonar.host.url="http://localhost:9000" /d:sonar.token="sqp_690ee48385538266c23a6dc72c2829f0e2a30d53"

# Step 3: Build the project
echo "Building the project..."
dotnet build

# Step 4: End SonarQube analysis
echo "Ending SonarQube analysis..."
dotnet sonarscanner end /d:sonar.token="sqp_690ee48385538266c23a6dc72c2829f0e2a30d53"

echo "SonarQube analysis completed."

sleep 10
