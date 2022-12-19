# Todo
List of issues that can be worked to deliver the desired player experience categorised by the design pillars. These can be things that were observed during the playtesting or other feedback and observations.

## Gameplay
1. Fun and useful emotions


## UI
- [x] Add main menu UI
- [ ] Add pause UI

## Levels
1. Smart
   1. Jump over obstacle
   2. Open a path to goal
   3. Wind that needs to be slowed down
   
## Design problems
1. Levels with bouncing obstacles are too easy, because sometimes emotions are able to move too fast
  1. Tuned the responsiveness of emotion
2. Unity remote and device gyro sensors use different axis
   1. Added handling for different axises on Unity remote (y) and build version (x)
3. Players sometimes skip parts of the level
  1. How could I disencourage skipping?
     1. Secondary goal(s)
     2. Blocking the skips with obstacles
4. The emotion(s) aren't identifiable 
5. Players aren't able to identify how many emotions they still need to process
6. Succeeding in improving the mood doesn't provide fun feedback
