--[[
	This is the test code for the RPG
]]

local map = require("map")
local controls = require("controls")
local networking = require("networking")
local entity = require("entity")
local user = require("player")
local chat = require("chat")

printChat("Server", "Your mother is mine")
printChat("BobBarker", "I will eat her")
printChat("SallySue", "Stop talking about my mom, that's mean!")
print(controls)
--Here's a few global variables

function love.load()
	networkInit()
end

function love.update(dTime)
	check_keyboard(dTime)
	networkUpdate()
end

function love.draw()
	--Draws entities, the player, and obstacles
	for i=1, #entities do
		love.graphics.setColor(0,255,0)
		love.graphics.rectangle("fill",entities[i].posX, entities[i].posY, player.scale*6.4, player.scale*6.4)
	end
	love.graphics.setColor(255,255,255)
	love.graphics.rectangle("fill",player.posX, player.posY, player.scale*6.4, player.scale*6.4)
	drawObstacles()
	
	--Player attack
	if player.state == "attacking" then
		player.state = "neutral"
		love.graphics.setColor(30,50,100)
		love.graphics.circle("fill", player.posX+player.scale*3.2, player.posY-10, 10)
		--Remove this when done testing
		for i=1, #chatLog do
			print(chatLog[i])
		end
	end
	
	--Chat
	love.graphics.setColor(255,255,255)
	renderChat()
end

function drawObstacles()
	if #collisionMap > 0 then
		love.graphics.setColor(255,0,0)
		for i=1, #collisionMap do
			love.graphics.rectangle("fill", collisionMap[i][1], collisionMap[i][2], 6.4*player.scale, 6.4*player.scale)
		end
	end
end



