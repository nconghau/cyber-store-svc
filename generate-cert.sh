#!/bin/bash

# Set certificate paths
CERT_DIR="Private"
CERT_KEY="$CERT_DIR/certificate.key"
CERT_CRT="$CERT_DIR/certificate.crt"
CERT_PFX="$CERT_DIR/certificate.pfx"
PFX_PASSWORD="cyber_store"

# Create the Private directory if it doesn't exist
mkdir -p "$CERT_DIR"

# Generate a self-signed SSL certificate
openssl req -x509 -newkey rsa:2048 -keyout "$CERT_KEY" -out "$CERT_CRT" -days 365 -nodes -subj "/CN=localhost"

# Convert the certificate and private key into a PFX file
openssl pkcs12 -export -out "$CERT_PFX" -inkey "$CERT_KEY" -in "$CERT_CRT" -passout pass:$PFX_PASSWORD

# Verify the PFX file
openssl pkcs12 -info -in "$CERT_PFX" -nokeys -passin pass:$PFX_PASSWORD

# Output the generated files
echo "✅ Certificates generated:"
ls -l "$CERT_DIR"

echo "📌 Use the following in your ASP.NET Core app:"
echo "listenOptions.UseHttps(\"$CERT_PFX\", \"$PFX_PASSWORD\");"
