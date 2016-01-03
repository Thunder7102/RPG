local socket = require "socket"

local serverAddress, serverPort = "185.72.161.147", 9050
server = nil
--local lastTickTime, tickThreshold = 1/30, 1/30
--[[
local entities = {}
local player = {
	x = 50,
	y = 50
};]]

function networkInit()
	server = socket.udp()
	server:setpeername(serverAddress, serverPort)
	server:settimeout(0)
end

function networkUpdate()
	data, msg = server:receive()
	if data then
		local parts = {}
		for token in string.gmatch(data, "[^%s]+") do
			table.insert(parts, token)
		end
		-- ID TYPE A1 A2 A3
		if parts[2] == "1" then -- login
			findOrCreateEntity(parts[1])
		end
		if parts[2] == "2" then -- logout
			local entity, index = findOrCreateEntity(parts[1])
			table.remove(entities, index)
		end
		if parts[2] == "100" then -- move
			local entity = findOrCreateEntity(parts[1])
			entity.posX = tonumber(parts[3])
			entity.posY = tonumber(parts[4])
		end
	end
	if msg and msg ~= "timeout" then print(msg) end

end

