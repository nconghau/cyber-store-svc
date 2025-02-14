#!/bin/bash

if ! docker info > /dev/null 2>&1; then
    echo "Docker is not running!"
    exit 1
fi

# Check if the SVC_VERSION is provided
if [ -z "$1" ]; then
    echo "Error: No SVC_VERSION tag provided."
    exit 1
fi

# Assign the SVC_VERSION to a variable
SVC_VERSION=$1
IMAGE_NAME="haucoder/cyber-store-svc"

# Check if the SVC_VERSION string contains 'dev' or 'prod'
if [[ "$SVC_VERSION" == *"-dev"* ]]; then
    BUILD_ENV="dev"
    echo "🔹 Building for development with SVC_VERSION: $SVC_VERSION..."
    docker build -t $IMAGE_NAME:$SVC_VERSION .
    echo "✅ Build completed with SVC_VERSION $SVC_VERSION for environment $BUILD_ENV!"

elif [[ "$SVC_VERSION" == *"-prod"* ]]; then
    BUILD_ENV="prod"
    echo "🔹 Building for production with SVC_VERSION: $SVC_VERSION..."
    docker buildx build --platform linux/amd64 -t $IMAGE_NAME:$SVC_VERSION .
    echo "✅ Build completed with SVC_VERSION $SVC_VERSION for environment $BUILD_ENV!"
    # docker login --username=haucoder
    docker push $IMAGE_NAME:$SVC_VERSION
    
else
    echo "Error: Invalid SVC_VERSION format. Use a SVC_VERSION with '-dev' or '-prod' suffix."
    exit 1
fi
