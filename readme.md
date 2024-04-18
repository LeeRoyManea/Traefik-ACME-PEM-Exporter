Traefik ACME PEM Exporter

This dotnet application exports PEM certificates from Traefik's acme.json file.
Requirements

    Docker installed on your system

Usage

    Pull the Docker image:

    
```bash
docker pull <image_name>
```


Set the following environment variables:

    ExportPath: Path where the PEM certificates will be exported.
    AcmePath: Path to the acme.json file.
    TimeInterval: Time interval in minutes for exporting certificates.

Example:

```bash
export ExportPath=/path/to/export
export AcmePath=/path/to/acme.json
export TimeInterval=5
```

Run the Docker container:

bash

    docker run -e ExportPath=$ExportPath -e AcmePath=$AcmePath -e TimeInterval=$TimeInterval <image_name>

The application will automatically export PEM certificates from the specified acme.json file to the provided export path at the specified time interval.
License

This project is licensed under the MIT License.