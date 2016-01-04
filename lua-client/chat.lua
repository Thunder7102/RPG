--[[
	Will manage the chat window and various interface
]]

--This will make the log start near the bottom of the screen and will only go halfway across the screen
scrWidth = love.graphics.getWidth()
scrHeight = love.graphics.getHeight()

chatHeight = scrHeight*.7
chatWidth = scrWidth*.6

chatLog = {}

function printChat(sender, text)
	if text == nil then 
		text = sender
		sender = "Console"
	end
	
	local message = "<"..sender..">   "..text
	table.insert(chatLog, message)
end

function renderChat()
	local index = 0
	for i = #chatLog, 1, -1 do
		love.graphics.print(chatLog[i], 20, chatHeight - (index * 15))
		index = index + 1
		if index >= 10 then break end
	end

	--[[
	--Pulling oldest entries in chatLog and filling textBox
	local textBox = {}
	local k = 0
	for i=10, 1, -1 do
		if #chatLog > k then
			table.insert(textBox, chatLog[#chatLog-k])
			k=k+1
		end
	end
	
	--Now to render textBox
	for i=1, #textBox do
		love.graphics.print(textBox[i], 20, chatHeight - ((#textBox - i)*15))
	end
	]]
end

function renderInputBox()
	--Creates a togglable switch
	if player.state == "chat" then
		player.state = "neutral"
	elseif player.state == "chatstart" then
		love.graphics.setColor(0,0,0)
		love.graphics.rectangle("fill",20, chatHeight+20,300,20)
		love.graphics.setColor(255,255,255)
		love.graphics.rectangle("line",20, chatHeight+20,300,20)
		love.graphics.print("|", 23, chatHeight+20)
		
	end
end
