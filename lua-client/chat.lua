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
	for i=#textBox, 1, -1 do
		love.graphics.print(textBox[i], 20, chatHeight + ((#textBox - i)*15))
	end
end

