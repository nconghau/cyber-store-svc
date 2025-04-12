#!/bin/bash

SVC_VERSION=$1
NUM_INSTANCES=$2

if [ -z "$SVC_VERSION" ]; then
  echo "❌ ERROR: Missing SVC_VERSION. Usage: ./vps-scale-swarm.sh <SVC_VERSION> <NUM_INSTANCES>"
  exit 1
fi

if [ -z "$NUM_INSTANCES" ]; then
  NUM_INSTANCES=1  # Default to 1 instance if not provided
fi

echo "✅ Exporting SVC_VERSION=$SVC_VERSION"
export SVC_VERSION

# Optionally, redeploy the stack (if needed) or assume it's already deployed
echo "🚀 Deploying stack (if not already deployed)..."
docker stack deploy -c docker-swarm.yml svc

echo "✅ Waiting for services to initialize..."
sleep 5

echo "✅ Scaling svc_cyber-store-svc to $NUM_INSTANCES instances..."
docker service scale svc_cyber-store-svc=$NUM_INSTANCES

echo "✅ Deployment and scaling complete..."
docker service ls