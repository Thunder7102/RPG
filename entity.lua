--[[
	The base of entities and core functions they will have 
	access to.
]]
entities = {}

local entity = {
	--The default stats of an entity, can be changed/added/removed later
	hp = 1,
	--p stands for physical defense, m is magical
	pDefense = 1, 
	mDefense = 1,
	pDamage = 1, 
	mDamage = 1, 
	energy = 1,
	speed = 1,
	--More technical properties
	size = 1, 
	map = 0, 
	posX = 1, 
	posY = 1,
	id = 1, 
	facing = 1
}

function findOrCreateEntity(id)
	id = tonumber(id)
	for _, ent in pairs(entities) do
		if ent.id == id then return ent, _ end
	end
	ent = {
		id = id,
		posX = 0,
		posY = 0
	}
	table.insert(entities, ent)
	return ent, #entities
end


