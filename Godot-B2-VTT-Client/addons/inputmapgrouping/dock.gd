tool
extends Control


# Declare member variables here. Examples:
# var a = 2
# var b = "text"


# Called when the node enters the scene tree for the first time.
func _ready():
	print("aaa?");


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass



func _on_Button_pressed():
	var dict = {}
	var props = ProjectSettings.get_property_list();

	print("OK");
	for E in props:
		var name = E.name;
		if !name.begins_with("input/"):
			continue;
		var action_name = name.substr(("input/").length());
		var split = action_name.split("_", true,1);
		
		if dict.has(split[0]):
			continue;

		dict[split[0]] = action_name;
		

		print(split[0]);
	
	for group in dict:
		group.
		pass
