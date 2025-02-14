#!/bin/bash

SVC_VERSION=$1

echo "✅ Export SVC_VERSION=$SVC_VERSION"
export SVC_VERSION=$SVC_VERSION

docker-compose up -d
echo "✅ Deploying..."
