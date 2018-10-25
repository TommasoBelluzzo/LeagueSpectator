# LeagueSpectator

LeagueSpectator is a simple utility for spectating League of Legends matches.

## Documentation

### Compatibility

LeagueSpectator can run on every machine equipped with Windows 7 or greater. The application requires administration privileges in order to work properly; a semi-automatic elevation routine is included in the application manifest file.

### Usage

![Interface](https://i.imgur.com/l2Ous1t.png)

Since this project is not meant to be massively distributed and exploited, its communication with Riot API endpoints doesn't rely on a global web service capable of proxying queries using a production API key. Instead, the protocol must be configured on a per-user basis by properly filling the `API Key` and `API Version` textboxes. Temporary API keys are well suited for sporadic usage since every existing League of Legends account can obtain them through the [Riot Developer Portal](https://developer.riotgames.com/).

### Source Code

Methods and classes are completely undocumented and the code contains no descriptive comments; I know it's a bad practice, but I really don't have time to create a proper documentation for this project. Nevertheless, everything should be pretty straightforward since it's just a simple form with a few buttons.
