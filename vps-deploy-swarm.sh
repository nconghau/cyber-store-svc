#!/bin/bash

# Load environment variables from .env file
if [ -f .env ]; then
  echo "✅ Loading environment variables from .env file..."
  set -a
  source .env
  set +a
else
  echo "❌ ERROR: .env file not found!"
  exit 1
fi

SVC_VERSION=$1

if [ -z "$SVC_VERSION" ]; then
  echo "❌ ERROR: Missing SVC_VERSION. Usage: ./vps-deploy-swarm.sh <SVC_VERSION>"
  exit 1
fi

echo "✅ Exporting SVC_VERSION=$SVC_VERSION"
export SVC_VERSION

echo "🚀 Deploying stack..."
docker stack deploy -c docker-swarm.yml svc

echo "✅ Waiting for services to initialize..."
sleep 5
docker service ls

echo "✅ Networks:"
docker network ls

echo "✅ Deployment complete..."

