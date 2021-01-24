# tetris-ai

# Introduction
Tetris looked like the perfect game to test a powerfull machine learning ai. The goal of this project is to test the vaibility of machine learning in a realy basic tertis game.

# Design/implementation
## The making of my tetris game
First thing on the todo list is decide to use a game engine or to make it completely from scratch. I decided that using unity is the best option because of the ease of use and I can finish the game way faster in unity. To keep it simple I just made block prefabs of al the tetris blocks. Then I made a block script that lowers them down. once they reach the bottom they get added to a static grid and are disabled. If a block moves down onto the grid grid it also get added to the grid and disabled. Then I made a spawner script that gets called by the block and spawns a new one every time one gets disabled and 1 time at the start of the game. Finaly every time a block gets placed a check is run to see if there is a row. If there is a row it is deleted and all block above it are moved down.

![](/Images/tetrisWorks.gif)  

## Reasearching machine learning
After a bit of research I found a document [Here](https://melax.github.io/tetris/tetris.html) about using reinforcement learning for tetris. After a few hours trying to understand it I still couldn't grasp anything usefull written there so I kept looking. Then I found a unity package [Here](https://github.com/Unity-Technologies/ml-agents) that made using machine learning easy to use in unity. ml-agents can use 2 type of machine learning PPO(Proximal Policy Optimization) or SAC(Soft Actor Critic). I decided to use PPO 
bacause PPO is more suited for a where it has to make al lot decisions continuously. SAC is more suited for boardgames with a slower pace. I is also less stable.

## implementing machine learning
To start implementing the ai. I had to refactor some parts of the tetris code. As you might have noticed I forgot to make a gameover state. I also had to give some information to the the ml-agent and reward the agent for good moves. To fix this I added a game manager script. The manager script now holds the grid of blocks and the grid itself is no longer static to allow more than one game to run at once (for faster learning). I added a score system to the game manager script. The manager gives all the needed information to the ml agent.

![](/Images/AIObservatiobs.png)  

This includes the grid of blockes and the current block. The game manager rewards the ml agent for "good moves" the bot gains. A good move is decided by the movescore. The movescore gets increased by 10 if the placed block does not increase the total height of the grid. The movescore also gets increased by 10 if the placed block does not create new holes(a empty space with a block above it).

![](/Images/blockReward.png) 

The ai is also rewarded 1000 points for removing a row.

![](/Images/AIrewardRow.png)

The ml-agent gives 2 numbers that decide the actions it wants to take. The first is a number between 0 and 3: 0 do nothing, 1 move left, 2 move right and 3 rotate. the second number is always between 0 and 1 like the first number 0 equals no move. And if the number is 1 fallingspeed of the block is increased.

![](/Images/agentMovement.png)

Now all of this is programmed the ai can almost start learning. the only thing left to do is make a config file.

![](/Images/config.png)

now the config file is done you can start the learning proces via powershell.

![](/Images/startTraining.png)

# Conclusion/Future work
The result is disappointing. This is a bot that has trained 3 times for 20e6 steps.

 ![Result](/Images/aiNotSmart.gif)
 
 I think there are 2 main reasons the ai is not learning correctly. Reason 1 I think the reward system. The ai gets rewarded too often for random moves and doesn't get rewarded enough for good moves. The way to fix this is to add extra ways to reward the ai. Maybe adding reward for good moves with less inputs. The other porblem has to dowith my implementation that of tetris. There is a maximum amount of inputs/sec because of this the ai gives inputs that do not get registered. this causes the ai to think it made moves that did not happen.
