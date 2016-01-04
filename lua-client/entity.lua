--[[
	The base of entities and core functions they will have 
	access to.
]]
entities = {} 
projectiles = {}

function createProjectile(x,y,dir, owner)
	local projectile = {
		posX = x,
		posY = y, 
		boxWidth = 1, 
		boxHeight = 1,
		parent = "",
		speed = 100,
		
		-- predictive movement variables
		lastDirectionTime = love.timer.getTime(),
		dir = dir
	}
	
	projectile.calculatePosition = function()
		local dTime = love.timer.getTime() - projectile.lastDirectionTime
		local posX = projectile.posX + math.cos(projectile.dir) * projectile.speed * dTime
		local posY = projectile.posY + math.sin(projectile.dir) * projectile.speed * dTime
		return posX, posY
	end
	
	table.insert(projectiles, projectile)
	return projectile
end

function createEntity()

	local entity = {
		--The default stats of an entity, can be changed/added/removed later
		hp = 25,
		maxhp = 25,
		--p stands for physical defense, m is magical
		pDefense = 1, 
		mDefense = 1,
		pDamage = 1, 
		mDamage = 1, 
		energy = 10,
		maxEnergy = 10,
		speed = 100,
		--More technical properties
		size = 1, 
		map = 0, 
		posX = 1, 
		posY = 1,
		id = 1, 
		facing = 0, --0 is forward, facing is handled via degrees
		
		-- predictive movement variables
		lastDirectionTime = 0,
		dirX = 0,
		dirY = 0
	}
	
	entity.calculatePosition = function()
		if entity.dirX == 0 and entity.dirY == 0 then return entity.posX, entity.posY end
		local posX, posY = entity.posX, entity.posY
		local dTime = love.timer.getTime() - entity.lastDirectionTime
		posX = posX + entity.dirX * dTime
		posY = posY + entity.dirY * dTime
		return posX, posY
	end
	
	table.insert(entities, entity)
	return entity
end

function findOrCreateEntity(id)
	id = tonumber(id)
	
	if id == player.id then return player end
	
	for _, ent in pairs(entities) do
		if ent.id == id then return ent, _ end
	end
	print("Creating entity " .. id .. " at index " .. (#entities + 1))
	ent = createEntity()
	ent.id = id
	return ent, #entities
end


