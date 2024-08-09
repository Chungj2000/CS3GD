# CS3GD
Repository for Game Development coursework.
**Please Note:** Leaderboard is defunct due to Backendless trial expiring.

# Purpose
Development of a game built on Unity Engine that meet a set of requirements listed by the stakeholder.
The final product is expected to feature **wave-based shooter** gameplay that keeps track of the players score based on their performance. The score should be persistent, allowing for the creation of a leaderboard. Finally environment should be immersive and dynamic based ton the player's input.

## Objectives
- [x] Create a Main Menu that features a functional leaderboard.
- [x] Implement a player model that responds to player input.
  - [x] Set up a camera following ARPG conventions using Cinemachine.
  - [x] Attach animations to the player model based on their state.
- [x] Create enemies for the player to interact with.
  - [x] Implement enemy AI behaviour.
  - [x] Attach animations to the enemy model based on their state.
- [x] Implement combat logic between player and enemy.
  - [x] Add parameter values to enemy and player that determine combat effectiveness.
  - [x] Implement shooting/projectile logic of player attack input.
  - [x] Implement visual indicators for combat parameters such as health.
- [x] Implement enemy wave logic.
  - [x] Generate a number of enemies based on the current wave.
  - [x] End the current wave when the enemies are reduced to 0.
  - [x] Assign a new wave and increase the difficulty after a wave is ended.
  - [x] Implement game over logic for when the player dies.
- [x] Implement scoring component.
  - [x] Implement score logic for internal tracking and visual components.
  - [x] Create a database using [```Backendless```](https://backendless.com/).
  - [x] Implement logic for storing and manipulating persistent data stored in the database.
  - [x] Connect the scoring database to the leaderboard.
- [x] Design a level for players to interact with.
  - [x] Create a traversable environment for the player model using NavMesh.
  - [x] Implement interactable components such as traps and doors.
- [x] Implement features outside the scope of the requirements that would improve player experience.
  - [x] Add sound.
  - [x] Implement item logic to update player attributes.
    - [x] Create an item handler for updating parameter values.
    - [x] Create a set of items.
  - [x] Create a pause menu.
    - [x] Implement options on pause menu and main menu.
      - [x] Sound options.
      - [x] Resolution options.
      - [ ] Implement customisable input mapping for various devices.
    - [x] Implement save/load functionality.
  - [ ] Create a mini map.
  - [ ] Enhance gameplay.
    - [ ] Add challenge bosses.
    - [ ] Create an enemy pool that spawns more types of enemies as the player progresses through waves.

### Trello Link for Development Log:
Public archive of tasks associated with the projects development. <br />
[Trello Board](https://trello.com/invite/b/IPfJdFOo/ATTIe1d90d8095eb3c6b7102d86b9f4be91a7FEDAE96/cs3gd-project) 
