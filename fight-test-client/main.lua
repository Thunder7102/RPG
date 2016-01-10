local player = {}
local enemies = {}
local projectiles = {}
local mouseWasPressed = false
local dashWasPressed = false
local deathFadeoutTime = 3
local dashMultiplier = 7.5
local dashTime = 0.1

function love.load()
	player = createPlayer()
	for i = 1, 3 do table.insert(enemies, createEnemy()) end
end

function love.update(dTime)
	for _, enemy in pairs(enemies) do
		if not enemy.dying then
			enemy.angle = angleBetween(enemy, player)
			enemy.x = enemy.x + math.cos(enemy.angle) * enemy.speed * dTime
			enemy.y = enemy.y + math.sin(enemy.angle) * enemy.speed * dTime
		elseif love.timer.getTime() - enemy.dying > deathFadeoutTime then
			table.remove(enemies, _)
		end
	end
	
	if #enemies < 3 then
		table.insert(enemies, createEnemy()) 
	end
	
	local mouseX, mouseY = love.mouse.getPosition();
	player.angle = angleBetween(player, { x = mouseX, y = mouseY })
	
	local direction = {x = 0, y = 0}
	
	if love.keyboard.isDown('up') or love.keyboard.isDown('w') then
		direction.y = direction.y - player.speed * dTime
	end
	if love.keyboard.isDown('right') or love.keyboard.isDown('d') then
		direction.x = direction.x + player.speed * dTime
	end
	if love.keyboard.isDown('down') or love.keyboard.isDown('s') then
		direction.y = direction.y + player.speed * dTime
	end
	if love.keyboard.isDown('left') or love.keyboard.isDown('a') then
		direction.x = direction.x - player.speed * dTime
	end
	if love.keyboard.isDown('space') then
		if not dashWasPressed and not player.dashing then
			player.dashing = love.timer.getTime() + dashTime
		end
		dashWasPressed = true
	else 
		dashWasPressed = false
	end
	
	if direction.x ~= 0 and direction.y ~= 0 then
		direction.x = direction.x / math.sqrt(2)
		direction.y = direction.y / math.sqrt(2)
	end
	
	if player.dashing then
		if love.timer.getTime() > player.dashing then player.dashing = nil 
		else
			direction.x = direction.x * dashMultiplier
			direction.y = direction.y * dashMultiplier
		end
	end
	
	player.x = player.x + direction.x
	player.y = player.y + direction.y
	
	if love.mouse.isDown(1) then
		if not mouseWasPressed and #projectiles == 0 then
			table.insert(projectiles, createSwordProjectile(player))
		end
		mouseWasPressed = true
	else 
		mouseWasPressed = false
	end
	
	for index, projectile in pairs(projectiles) do
		if projectile.isExpired() then
			table.remove(projectiles, index)
		end
	end
	
	for _, projectile in pairs(projectiles) do
		for index, enemy in pairs(enemies) do
			if projectileHit(projectile, enemy) then
				hitBackEnemy(enemy)
				enemy.health.current = enemy.health.current - 1
				if enemy.health.current <= 0 then
					enemy.dying = love.timer.getTime()
				end
			end
		end
	end
end

function projectileHit(projectile, enemy)
	local hitpoints = projectile.hitPoints()
	for _, hitpoint in pairs(hitpoints) do
		if distanceBetween(hitpoint, enemy) < math.sqrt(25*25) then return true end
	end
	return false
end

function hitBackEnemy(enemy) 
	local angleBetweenCenter = angleBetween(player, enemy)
	enemy.x = enemy.x + enemy.speed * math.cos(angleBetweenCenter)
	enemy.y = enemy.y + enemy.speed * math.sin(angleBetweenCenter)
end

function distanceBetween(obj1, obj2)
	return math.sqrt(math.pow(obj1.x - obj2.x, 2) + math.pow(obj1.y - obj2.y, 2))
end

function drawRectangle(x, y, angle, distance, width, height)
	love.graphics.push()
	love.graphics.translate(x, y)
	love.graphics.rotate(angle)
	love.graphics.translate(-x, -y + distance)
	love.graphics.rectangle("fill", x - width / 2, y - height / 2, width, height)
	love.graphics.pop()
end

function love.draw()
	for _, enemy in pairs(enemies) do
		local opacity = 255
		if enemy.dying then
			local elapsedTime = love.timer.getTime() - enemy.dying
			opacity = 255 / elapsedTime / deathFadeoutTime
		end
		love.graphics.setColor(255, 0, 0, opacity)
		drawRectangle(enemy.x, enemy.y, enemy.angle, 0, 50, 50);
	end
	for _, projectile in pairs(projectiles) do
		love.graphics.setColor(255, 255, 0)
		drawRectangle(projectile.owner.x, projectile.owner.y, projectile.calculateAngle() - math.pi / 2, projectile.distance, projectile.width, projectile.height)
		love.graphics.setColor(255, 255, 255)
		
		local hitPoints = projectile.hitPoints()
		for _, hitpoint in pairs(hitPoints) do
			drawRectangle(hitpoint.x, hitpoint.y, 0, 0, 5, 5)
		end
	end
	love.graphics.setColor(0, 255, 0)
	drawRectangle(player.x, player.y, player.angle, 0, 50, 50);
end

function createEnemy()
	local angle = love.math.random() * math.pi * 2
	local e = {
		x = love.graphics.getWidth() / 2 + math.cos(angle) * love.graphics.getWidth() / 2,
		y = love.graphics.getHeight() / 2 + math.sin(angle) * love.graphics.getHeight() / 2,
		health = createAttribute('health', 2),
		angle = 0,
		speed = 100
	}
	return e
end

function angleBetween(obj1, obj2)
	return math.atan2(obj2.y - obj1.y, obj2.x - obj1.x)
end

function createPlayer()
	local p = {
		x = love.graphics.getWidth() / 2,
		y = love.graphics.getHeight() / 2,
		angle = 0,
		speed = 100,
		health = createAttribute('health', 10)
	}
	return p
end

function createSwordProjectile(player)
	local swordProjectile = {
		owner = player,
		startAngle = player.angle - math.pi / 4,
		endAngle = player.angle + math.pi / 4,
		distance = 50,
		width = 5,
		height = 50,
		startTime = love.timer.getTime(),
		endTime = love.timer.getTime() + 0.25
	}
	
	swordProjectile.calculateAngle = function()
		local angleDifference = swordProjectile.endAngle - swordProjectile.startAngle
		local timeDifference = swordProjectile.endTime - swordProjectile.startTime
		
		local percentage = (love.timer.getTime() - swordProjectile.startTime) / timeDifference
		return angleDifference * percentage + swordProjectile.startAngle
	end
	swordProjectile.didHit = function(target)
	end
	swordProjectile.hitPoints = function()
		local result = {};
		local hitpointCount = 5
		local angle = swordProjectile.calculateAngle()
		for i = 1, hitpointCount do
			local x = swordProjectile.owner.x + math.cos(angle) * ((swordProjectile.distance + swordProjectile.height / 2) * (i / hitpointCount))
			local y = swordProjectile.owner.y + math.sin(angle) * ((swordProjectile.distance + swordProjectile.height / 2) * (i / hitpointCount))
			table.insert(result, { x = x, y = y})
		end
		return result
	end
	swordProjectile.isExpired = function() return love.timer.getTime() > swordProjectile.endTime end
	return swordProjectile
end

function createAttribute(name, maxAmount)
	return {
		max = maxAmount,
		current = maxAmount,
		name = name
	}
end
