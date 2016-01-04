local socket = require "socket"
function math.round(n, deci) deci = 10^(deci or 0) return math.floor(n*deci+.5)/deci end

local serverAddress, serverPort = "185.72.161.147", 9050
--local serverAddress, serverPort = "localhost", 9050
server = nil
local lastPingTime, pingInterval = 1, 1
local receivedPingResult = true
--local lastTickTime, tickThreshold = 1/30, 1/30
--[[
local entities = {}
local player = {
	x = 50,
	y = 50
};]]

function isnan(x) return x ~= x end

function networkInit()
	server = socket.udp()
	server:setpeername(serverAddress, serverPort)
	server:settimeout(0)
	server:send("1") -- authenticate
end


function updateEntityPosition(entity, parts)
	if #parts < 4 then return end
	local posX = tonumber(parts[3])
	local posY = tonumber(parts[4])
	
	print("Entity " .. entity.id .. " at " .. posX .. " / " .. posY)
	
	if isnan(posX) or isnan(posY) then return end
	entity.posX = posX
	entity.posY = posY
	entity.lastDirectionTime = love.timer.getTime()
end

function updateEntityPositionAndDirection(entity, parts)
	if #parts < 6 then return end
	local posX = tonumber(parts[3])
	local posY = tonumber(parts[4])
	local dirX = tonumber(parts[5])
	local dirY = tonumber(parts[6])
	
	print("Entity " .. entity.id .. " at " .. posX .. " / " .. posY)
	
	if isnan(posX) or isnan(posY) or isnan(dirX) or isnan(dirY) then return end
	entity.posX = posX
	entity.posY = posY
	entity.dirX = dirX
	entity.dirY = dirY
	entity.lastDirectionTime = love.timer.getTime()
end

function networkUpdate(dTime)
	lastPingTime = lastPingTime + dTime
	if lastPingTime > pingInterval then
		lastPingTime = lastPingTime - pingInterval
		if receivedPingResult then 
			server:send("4\n")
			receivedPingResult = false
		end
	end
	data, msg = server:receive()
	if data then
		local parts = {}
		for token in string.gmatch(data, "[^%s]+") do
			table.insert(parts, token)
		end
		-- ID TYPE A1 A2 A3
		if parts[2] == "1" then -- Authenticated
			-- ID 1 X Y
			player.id = tonumber(parts[1])
			print("Player ID is " .. parts[1])
			updateEntityPosition(player, parts);
		end
		if parts[2] == "2" then -- login
			-- ID 2 X Y
			-- ID 2 X Y DirX DirY
			local entity = findOrCreateEntity(parts[1])
			if #parts > 4 then updateEntityPositionAndDirection(entity, parts, 3) 
			else updateEntityPosition(entity, parts); end
			print("Entity " .. parts[1] .. " added")
		end
		if parts[2] == "3" then -- logout
			-- ID 3
			local entity, index = findOrCreateEntity(parts[1])
			table.remove(entities, index)
			print("Entity " .. parts[1] .. " removed at index " .. index)
		end
		if parts[2] == "100" then -- move
			-- ID 100 X Y DirX DirY
			local entity = findOrCreateEntity(parts[1])
			if entity ~= player then updateEntityPositionAndDirection(entity, parts, 3) end
		end
		if parts[2] == "200" then -- chat
			local msg = ""
			for i = 3, #parts do 
				if i > 3 then msg = msg .. " " end
				msg = msg .. parts[i] 
			end
			if parts[1] == "0" then printChat(msg)
			else printChat("User " .. parts[1], msg) end
		end
		if parts[2] == "4" then -- ping response
			print("Ping result in " .. math.round(lastPingTime * 100) .. " ms")
			receivedPingResult = true
			love.window.setTitle("Taylor and Victor's RPG Test - " .. math.round(lastPingTime * 100) .. " ms")
		end
	end
	if msg and msg ~= "timeout" then print(msg) end

end

