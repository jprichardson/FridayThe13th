Usage
=====

Let's say that you have this JSON:
	{
		"some_number": 108.541,
		"date_time": "2011-04-13T15:34:09Z",
		"serial_number": "SN1234"
		"more_data": {
			"field1": 1.0
			"field2": "hello"
		}
	}

You can deserialize it like so:
	using FridayThe13th;

	var jsonText = File.ReadAllText("mydata.json");

	var jsp = new JsonParser(){CamelizeProperties: true};
	dynamic json = jsp.Parse(jsonText);

	Console.WriteLine(json.SomeNumber); //outputs 108.541
	Console.WriteLine(json.MoreData.Field2); //outputs hello

License:
========

It's licensed under the LGPL V2.
