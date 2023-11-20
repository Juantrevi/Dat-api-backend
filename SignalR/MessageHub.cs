using AutoMapper;
using Dat_api.Data;
using Dat_api.DTOs;
using Dat_api.Entities;
using Dat_api.Extensions;
using Dat_api.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Dat_api.SignalR
{
	public class MessageHub : Hub
	{
		private readonly IMessageRepository _messageRepository;
		private readonly IUserRepository _userRepository;
		private readonly IMapper _mapper;

		public MessageHub(IMessageRepository messageRepository, IUserRepository userRepository, IMapper mapper)
        {
			_messageRepository = messageRepository;
			_userRepository = userRepository;
			_mapper = mapper;
		}


		public override async Task OnConnectedAsync()
		{
			var httpContext = Context.GetHttpContext();
			var otherUser = httpContext.Request.Query["user"].ToString();
			var groupName = GetGroupName(Context.User.GetUsername(), otherUser);
			await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

			var messages = await _messageRepository.GetMessageThread(Context.User.GetUsername(), otherUser);

			await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);

		}


		public override async Task OnDisconnectedAsync(Exception exception)
		{
			await base.OnDisconnectedAsync(exception);
		}



		private string GetGroupName(string caller, string other)
		{
			// CompareOrdinal is a method that compares two strings and returns an integer
			// that indicates their relative position in the sort order.
			var stringCompare = string.CompareOrdinal(caller, other) < 0; 

			// If the caller is less than the other user, then we will return the caller-other
			return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
		}

		public async Task SendMessage(CreateMessageDto createMessageDto)
		{
			var username = Context.User.GetUsername();

			if (username == createMessageDto.RecipientUsername.ToLower())
			{
				throw new HubException("You cannot send messages to yourself");
			}

			var sender = await _userRepository.GetUserByUserNameAsync(username);
			var recipient = await _userRepository.GetUserByUserNameAsync(createMessageDto.RecipientUsername);

			if (recipient == null)
			{
				throw new HubException("Not found user");
			}

			var message = new Message
			{
				Sender = sender,
				Recipient = recipient,
				SenderUsername = sender.UserName,
				RecipientUsername = recipient.UserName,
				Content = createMessageDto.Content
			};

			_messageRepository.AddMessage(message);

			if (await _messageRepository.SaveAllAsync())
			{
				var group = GetGroupName(sender.UserName, recipient.UserName);
				await Clients.Group(group).SendAsync("NewMessage", _mapper.Map<MessageDto>(message));

			}

		}
    }
}
