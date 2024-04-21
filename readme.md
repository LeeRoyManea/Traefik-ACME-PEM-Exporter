# Traefik ACME PEM Exporter

This dotnet application exports PEM certificates from Traefik's acme.json file.
## Requirements

Docker or a alternative for obvious reason

## Usage

You dont have to use the Environment Variables.
Just mount the volumes. The default values are described below.

If not changed, the *Traefik ACME PEM Exporter* export PEM certificates from the specified acme.json file to the provided export path every 15 minutes.

### Docker CLI

```bash
docker pull leeroymanea/dotnet-traefik-exporter:latest
docker run \
  -e TimeInterval=5 \
  -e AcmePath=/data/acme.json \
  -e ExportPath=/data/export \
  -v /path/to/acme.json:/data/acme.json \
  -v /path/to/export:/data/export \
  leeroymanea/dotnet-traefik-exporter:latest
```

### Docker Compose

```yml
version: '3'

services:
  traefik-exporter:
    image: leeroymanea/dotnet-traefik-exporter:latest
    environment:
      - TimeInterval=15
      - AcmePath=/data/acme.json
      - ExportPath=/data/export
    volumes:
      - /path/to/export:/data/export
      - /path/to/acme.json:/data/acme.json
```

The output will look like:
```
Starting new export
Exporting my.domain.com to /data/export/my.domain.com
Exporting my.second-domain.com to /data/export/my.second-domain.com
....
Certificate files exported successfully.
No new changes since 06:13:55 - 18.04.2024
No new changes since 06:13:55 - 18.04.2024
....
```

## License

This project is licensed under the [GPLv3](LICENSE).

## Support me 

If you want to support me, buy a [Coffee](https://ko-fi.com/leeroy_manea)

Thank you
