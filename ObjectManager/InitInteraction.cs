using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Class.ObjectManager;
using Mirror;
using UnityEngine;

public class InitInteraction : NetworkBehaviour
{
	public int choice;
	public GameObject player;
	public void StartInteraction()
	{
		var stringTag = transform.tag;
		var objectManager = player.GetComponent<ObjectManager>();
		var localObjectValue = player.GetComponent<LocalObjectValue>();
		
		//Calls a function according to the tag of the parent object
		if (stringTag == "Plot")
		{
			var selectedSeedInBag = -1;
					
			if (objectManager.equipedObjectId == ObjectId.TomatoSeedBag)
				selectedSeedInBag = localObjectValue.tomatoSeedsInBag;
			if (objectManager.equipedObjectId == ObjectId.CarrotSeedBag)
				selectedSeedInBag = localObjectValue.carrotSeedInBag;

			gameObject.GetComponent<Plot>().PlotInit(objectManager.equipedObjectId, player, selectedSeedInBag);
		}

		if (stringTag == "SeedDispenser")
		{
			gameObject.GetComponent<SeedDispenser>().StartInteraction();	
		}

		//The code below is for the pick-up object
		if (stringTag == "SeedBag")
		{
			//Check if an object is already equipped
			if (objectManager.objectEquiped)
			{
				//Set the toolEquiped variable to false
				CmdToolEquiped(0, objectManager);
				//Run DropTool to drop the equipped object
				objectManager.DropObject();
			}
			if (gameObject.GetComponent<ObjectIdentification>().objectName == "TomatoSeedBag")
			{
				//Method of object selection
				objectManager.ToolSelector(ObjectId.TomatoSeedBag);
				//Set toolEquiped variable to true
				CmdToolEquiped(1, objectManager);
				//Call the interaction method
				gameObject.GetComponent<TomatoSeedBag>().StartInteraction(player);
			}
			
			if (gameObject.GetComponent<ObjectIdentification>().objectName == "CarrotSeedBag")
			{
				objectManager.ToolSelector(ObjectId.CarrotSeedBag);
				CmdToolEquiped(1, objectManager);
				gameObject.GetComponent<CarrotSeedBag>().StartInteraction(player);
			}
		}
		//next object type
	}

	[Command(requiresAuthority = false)]
	private void CmdToolEquiped(int value, ObjectManager objectManager)
	{
		if (value == 0)
			objectManager.objectEquiped = false;
		if (value == 1)
			objectManager.objectEquiped = true;
	}
}
