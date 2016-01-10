--[[
	Separate player file for handling spells, inventory, stats, etc
]]


local entity = require("entity")
player = createEntity()
player.level = 1
player.scale = 10
player.state = "neutral"
print("Player speed is: "..player.speed)

function renderPlayerUI()
	--This will handle the basic UI of player energy and HP
	barSize = scrWidth/4
	
	--Here is the health bar
	love.graphics.setColor(138,145,148)
	love.graphics.rectangle("fill", barSize-10, scrHeight-((scrHeight*.04)+10), barSize, math.floor(scrHeight*.04))
	love.graphics.setColor(255,0,0)
	
	local healthInc = math.ceil(barSize/player.maxhp)
	if player.hp == player.maxhp then
		love.graphics.rectangle("fill", barSize-10, scrHeight-((scrHeight*.04)+10), barSize, math.floor(scrHeight*.04))
	elseif player.hp > 0 then
		love.graphics.rectangle("fill", barSize-10, scrHeight-((scrHeight*.04)+10), healthInc*player.hp, math.floor(scrHeight*.04))
	else
		player.hp = 0
	end
	
	love.graphics.setColor(255,255,255)
	love.graphics.print("HP: "..player.hp.." / "..player.maxhp, barSize*1.3, scrHeight-((scrHeight*.04)+10))
	
	--Here is the energy bar
	love.graphics.setColor(138,145,148)
	love.graphics.rectangle("fill", (barSize*2)+10, scrHeight-((scrHeight*.04)+10), barSize, math.floor(scrHeight*.04))
	
	love.graphics.setColor(0,0,255)
	local energyInc = math.ceil(barSize/player.maxEnergy)
	if player.energy == player.maxEnergy then
		love.graphics.rectangle("fill", (barSize*2)+10, scrHeight-((scrHeight*.04)+10), barSize, math.floor(scrHeight*.04))
	elseif player.energy > 0 then
		love.graphics.rectangle("fill", barSize*2, scrHeight-((scrHeight*.04)+10), energyInc*player.energy, math.floor(scrHeight*.04))
	else
		player.energy = 0
	end
	
	love.graphics.setColor(255,255,255)
	love.graphics.print("Energy: "..player.energy.." / "..player.maxEnergy, barSize*2.3, scrHeight-((scrHeight*.04)+10))
end

function boundingBox(x1,y1,w1,h1, x2,y2,w2,h2)
  ---Simple function to check if there's a collision between bounding boxes
  --DEV NOTE: Optimize later
  return x1 < x2+w2 and 
			 x2 < x1+w1 and
			 y1 < y2+h2 and
			 y2 < y1+h1
end

--Attacks
function math.angle(x1,y1, x2,y2) 
	return math.atan2(y2-y1, x2-x1) 
end

function player.attack()
	if player.state == "attacking" then
		local cursX, cursY = love.mouse.getPosition()
		print(math.angle(player.posX, player.posY, cursX, cursY))
		local attack = createProjectile(player.posX, player.posY, math.angle(player.posX, player.posY, cursX, cursY), player)
		player.state = "neutral"
	end
end

function player.update()
	player.attack()
end