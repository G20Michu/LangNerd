# Uruchamianie aplikacji LangNerd Web Client w Docker

## Wymagania wstępne

- Docker (wersja 20.10 lub nowsza)
- Docker Compose (wersja 2.0 lub nowsza)

## Szybkie uruchomienie

### Metoda 1: Docker Compose (zalecana)

```bash
# Zbuduj i uruchom aplikację
docker-compose up --build

# Uruchom w tle
docker-compose up -d --build

# Zatrzymaj aplikację
docker-compose down
```

Aplikacja będzie dostępna pod adresem: http://localhost:3000

### Metoda 2: Bezpośrednie polecenia Docker

```bash
# Zbuduj obraz
docker build -t langnerd-web-client .

# Uruchom kontener
docker run -p 3000:80 -e REACT_APP_API_URL=https://localhost:7262 langnerd-web-client

# Uruchom w tle
docker run -d -p 3000:80 -e REACT_APP_API_URL=https://localhost:7262 --name langnerd-web-client langnerd-web-client
```

## Konfiguracja

### Zmienne środowiskowe

Aplikacja używa następujących zmiennych środowiskowych:

- `REACT_APP_API_URL` - URL do API backendu (domyślnie: https://localhost:7262)

### Przykład konfiguracji

1. Skopiuj plik przykładowy:
```bash
cp env.example .env
```

2. Edytuj plik `.env`:
```bash
REACT_APP_API_URL=https://your-api-domain.com
```

3. Uruchom z custom URL:
```bash
docker-compose up --build
```

## Rozwój

### Build dla produkcji

```bash
# Zbuduj obraz produkcyjny
docker build -t langnerd-web-client:prod .

# Uruchom w trybie produkcyjnym
docker run -p 80:80 langnerd-web-client:prod
```

### Debugowanie

```bash
# Wejdź do działającego kontenera
docker exec -it langnerd-web-client sh

# Zobacz logi
docker-compose logs -f web-client
```

## Struktura plików Docker

- `Dockerfile` - Definicja obrazu Docker (multi-stage build)
- `docker-compose.yml` - Konfiguracja Docker Compose
- `nginx.conf` - Konfiguracja serwera nginx
- `.dockerignore` - Pliki wykluczane z buildu
- `env.example` - Przykładowe zmienne środowiskowe

## Porty

- **3000** - Port aplikacji web (mapowany na 80 w kontenerze)
- **80** - Port nginx wewnątrz kontenera

## Rozwiązywanie problemów

### Problem z połączeniem do API

Jeśli aplikacja nie może połączyć się z API:

1. Sprawdź czy zmienna `REACT_APP_API_URL` jest poprawnie ustawiona
2. Upewnij się, że backend API jest uruchomiony
3. Sprawdź czy nie ma problemów z CORS

### Problem z buildem

```bash
# Wyczyść cache Docker
docker system prune -a

# Zbuduj ponownie bez cache
docker-compose build --no-cache
```

### Sprawdzenie statusu

```bash
# Status kontenerów
docker-compose ps

# Logi aplikacji
docker-compose logs web-client
```
