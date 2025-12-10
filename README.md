# Renting

Renting to aplikacja webowa do zarządzania wypożyczalnią sprzętu IT, zbudowana w technologii ASP.NET Core Razor Pages (.NET 8).

## Funkcje

- Przeglądanie dostępnych zasobów IT (magazyn)
- Wypożyczanie sprzętu przez użytkowników
- Zarządzanie wypożyczeniami (zatwierdzanie, zwroty, anulowanie)
- Panel administracyjny dla zarządzania zasobami i wypożyczeniami
- Autoryzacja użytkowników (role: Admin, User)
- Powiadomienia o statusie wypożyczenia

## Technologie

- ASP.NET Core Razor Pages (.NET 8)
- Entity Framework Core
- Bootstrap 5
- SQL Server (domyślnie, można zmienić w konfiguracji)

## Uruchomienie lokalne

1. Sklonuj repozytorium:
   git clone https://github.com/Miloreq/Renting.git
2. Przejdź do katalogu projektu:
   cd Renting
3. Przygotuj bazę danych (np. migracje EF Core):
   dotnet ef database update
4. Uruchom aplikację:
   dotnet run
5. Otwórz przeglądarkę i przejdź pod adres `https://localhost:5001`

## Struktura projektu

- `Models` – modele danych (np. Asset, Rental, User)
- `Views` – widoki Razor Pages
- `Controllers` – logika kontrolerów
- `Data` – kontekst bazy danych
- `Services` – logika pomocnicza (np. SeedServices)
- `wwwroot` – zasoby statyczne (CSS, JS)

## Role użytkowników

- **Admin** – pełny dostęp do zarządzania zasobami i wypożyczeniami
- **User** – możliwość wypożyczania sprzętu i przeglądania własnych wypożyczeń

## Wymagania

- .NET 8 SDK
- SQL Server (lub inna kompatybilna baza danych)
- Visual Studio 2022/2026 lub VS Code

## Licencja

Projekt udostępniony na licencji MIT.

---

W razie pytań lub problemów, proszę o kontakt przez Issues na GitHub.
