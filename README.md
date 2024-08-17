# 🍸 BarKick

BarKick is a comprehensive management application designed to track football players, teams, venues, bartenders, and cocktails. It's the perfect solution for managing the intricate relationships between sports venues and their hospitality services.

## 🌟 Features

- 🏟️ Venue Management
- 🍹 Bartender Tracking
- ⚽ Team and Player Organization
- 🥂 Cocktail Recipe Database

## 📊 Entity Relationships

### Venue
- 🔑 Venue Id (int VenueID)
- 📍 Venue Name (string VenueName)
- 🗺️ Venue Location (string VenueLocation)
- 🏆 Teams that play there (ICollection Teams)
- 👨‍🍳 Bartenders working there (ICollection VenueBartenders)

### Bartender
- 🔑 Bartender Id (int BartenderId)
- 👤 First Name (string FirstName)
- 👤 Last Name (string LastName)
- 📧 E-mail (string Email)
- 🏢 Venues they work at (ICollection Venues)

### Team
- 🔑 Team Id (int TeamID)
- 🏅 Team Name (string TeamName)
- 📜 Team Biography (string TeamBio)
- 🏟️ Venues they play at (ICollection Venues)
- 👥 Players on the team (ICollection Players)

### Player
- 🔑 Player Id (int PlayerID)
- 👤 Player Name (string PlayerName)
- 🎽 Player Position (string PlayerPosition)
- 🏆 Team they play for (Team Team)

### Cocktail
- 🔑 Drink Id (int DrinkId)
- 🍸 Drink Name (string DrinkName)
- 📝 Drink Recipe (string DrinkRecipe)
- 🥃 Alcoholic ingredient (string LiqIn)
- 🧉 Non-alcoholic ingredient (string MixIn)
- 👨‍🍳 Bartender who created it (Bartender Bartender)
