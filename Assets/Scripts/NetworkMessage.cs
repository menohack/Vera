using System;
using UnityEngine;

namespace VeraLibrary
{

	/// <summary>
	/// Network messages stored using the Command pattern.
	/// </summary>
	interface NetworkMessage
	{
		void Execute();
		string Serialize();
		void Deserialize(string message);
	}

	public class NetworkMessageException : Exception { }
	public class MessageParseException : NetworkMessageException { }
	public class MessageCreationException : NetworkMessageException { }
	public class UninitializedMessageException : NetworkMessageException { }

	public class PositionMessage : NetworkMessage
	{
		string name;
		Transform transform;

		public PositionMessage(string type, string name, Transform transform)
		{
			this.name = name;
			this.transform = transform;
		}


		public PositionMessage(GameObject gameObject)
		{
			name = gameObject.name;
			transform = gameObject.transform;
		}

		/// <summary>
		/// Applies the message to the game.
		/// </summary>
		public void Execute()
		{
			if (name == null)
				throw new UninitializedMessageException();
			GameObject gameObject = GameObject.Find(name);

			if (gameObject != null)
			{
				gameObject.transform.position = transform.position;
				gameObject.transform.rotation = transform.rotation;
			}
		}

		/// <summary>
		/// Serializes a message into a string.
		/// </summary>
		/// <returns>A string representation of the message.</returns>
		public string Serialize()
		{
			return name + " " + transform.position.x + " " + transform.position.y + " " + transform.position.z
				+ " " + transform.rotation.x + " " + transform.rotation.y + " " + transform.rotation.z + " " + transform.rotation.w;
		}

		/// <summary>
		/// Constructs a PositionMessage object from a message.
		/// </summary>
		/// <param name="message">The string containing the message.</param>
		public void Deserialize(string message)
		{
			string[] tokens = message.Split(' ');
			if (tokens.Length != 8)
				throw new MessageParseException();
			int tokenCount = 0;
			name = tokens[tokenCount++];
			transform = new GameObject().transform;
			transform.position = new Vector3(float.Parse(tokens[tokenCount++]), float.Parse(tokens[tokenCount++]), float.Parse(tokens[tokenCount++]));
			transform.rotation = new Quaternion(float.Parse(tokens[tokenCount++]), float.Parse(tokens[tokenCount++]), float.Parse(tokens[tokenCount++]), float.Parse(tokens[tokenCount++]));
		}
	}
}
