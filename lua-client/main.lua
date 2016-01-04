--[[
	This is the test code for the RPG
]]

local map = require("map")
local controls = require("controls")
local networking = require("networking")
local entity = require("entity")
require("player")

local font = love.graphics.newFont(14)

print(controls)
--Here's a few global variables

function love.load()
	networkInit()
	love.graphics.setFont(font)
end

function love.update(dTime)
	check_keyboard(dTime)
	networkUpdate()
end

function drawEntity(x, y, id)
	local size = player.scale * 6.4
	love.graphics.rectangle("fill", x - size / 2, y - size / 2, size, size)
	love.graphics.setColor(0,0,0)
	love.graphics.printf(id, x - size / 2, y, size, 'center')
end

function love.draw()
	--Draws the player, then the obstacles
	for _, entity in pairs(entities) do
		if entity ~= player then
			x, y = entity.calculatePosition();
			love.graphics.setColor(0,255,0)
			drawEntity(x, y, entity.id)
			--love.graphics.rectangle("fill", x, y, player.scale*6.4, player.scale*6.4)
		end
	end
	
	--x, y = player.calculatePosition();
	love.graphics.setColor(255,255,255)
	drawEntity(player.posX, player.posY, player.id)
	drawObstacles()
end

function drawObstacles()
	local size = player.scale * 6.4
	if #collisionMap > 0 then
		love.graphics.setColor(255,0,0)
		for i=1, #collisionMap do
			love.graphics.rectangle("fill", collisionMap[i][1] - size / 2, collisionMap[i][2] - size / 2, size, size)
		end
	end
end



