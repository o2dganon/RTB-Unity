//brick2x4.cs


//Item
datablock ItemData(brick2x4 : brick2x2)
{
	shapeFile = "~/data/shapes/bricks/brick2x4.dts";
	pickUpName = 'a 2x4 brick';
	invName = "2x4";
	image = brick2x4Image;
	cost = 10;
};

//Static Shape
datablock StaticShapeData(staticBrick2x4 : staticBrick2x2)
{
	item = brick2x4;
	ghost = ghostBrick2x4;
	shapeFile = "~/data/shapes/bricks/brick2x4.dts";

	//lego scale dimensions
	x = 2;
	y = 4;
	z = 3;	//3 plates = 1 brick

	//boxX = 0.25;
	//boxY = 1.0;
	//boxZ = 0.3;
};


//Image
datablock ShapeBaseImageData(brick2x4Image : brick2x2Image)
{
	staticShape = staticBrick2x4;
	ghost = ghostBrick2x4;
	PreviewFileName = "rtb/data/shapes/bricks/Previews/Brick2x4.png";
	item = brick2x4;
};
