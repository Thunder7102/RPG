--[[
	Will manage the chat window and various interface
]]

chatLog = {}
--Messagebox will store the message the player is typing 
messageBox = ""

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
		love.graphics.print(chatLog[i], 20, chatHeight - (index * 20))
		index = index + 1
		if index >= 10 then break end
	end
end

function renderInputBox()
	--Renders the textbox and the text in the textbox
	if player.state == "chat" then
		love.graphics.setColor(0,0,0)
		love.graphics.rectangle("fill",20, chatHeight+20,300,25)
		love.graphics.setColor(255,255,255)
		love.graphics.rectangle("line",20, chatHeight+20,300,25)
		love.graphics.print(messageBox,23,chatHeight+20)
		if string.len(messageBox) < 40 then
			love.graphics.print("|", 23+font:getWidth(messageBox), chatHeight+20)
		end
	end
end
