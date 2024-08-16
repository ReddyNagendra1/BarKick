# BarKick
BarKick is a management app that tracks football players, teams, venues, the bartenders who work at those venues, and the cocktails they make.

## Entities
### Venue
The data points associated with each Venue are:
- Venue Id (int VenueID)
- Venue Name (string VenueName)
- Venue Location (string VenueLocation)
- The Teams that play there (ICollection<Team> Teams)
- The many Bartenders that work there (ICollection<VenueBartender> VenueBartenders)

### Bartender
The data points associated with each Bartender are:
- Bartender Id (int BartenderId)
- First Name (string FirstName)
- Last Name (string LastName)
- E-mail (string Email)
- The many Venues the Bartender works at (ICollection<VenueBartender> Venues)

### Team
The data points associated with each Team are:
- Team Id (int TeamID
- Team Name (string TeamName)
- Team Biography (string TeamBio)
- The many Venues the Team plays at (ICollection<Venue> Venues)
- The many Players that play on the Team (ICollection<Player> Players)

### Player
The data points associated with each Player are:
- Player Id (int PlayerID)
- Player Name (string PlayerName)
- Player Position (string PlayerPosition)
- The one Team that the Player plays for (Team Team)

### Cocktail
The data points associated with each Cocktail are:
- Drink Id (int DrinkId)
- Drink Name (string DrinkName)
- Drink Recipe (string DrinkRecipe
- The alcoholic ingredient (string LiqIn)
- The non-alcoholic ingredient (string MixIn)
- The one Bartender who created the Cocktail (Bartender Bartender)



