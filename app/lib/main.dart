import 'package:dio/dio.dart';
import 'package:flutter/material.dart';

void main() {
  runApp(CarRecommendationApp());
}

class CarRecommendationApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      debugShowCheckedModeBanner: false,
      title: 'Car Recommendation',
      theme: ThemeData(
        primarySwatch: Colors.blue,
      ),
      home: CarQuestionScreen(),
    );
  }
}

class CarQuestionScreen extends StatefulWidget {
  @override
  _CarQuestionScreenState createState() => _CarQuestionScreenState();
}

class _CarQuestionScreenState extends State<CarQuestionScreen> {
  int currentQuestionIndex = 0;
  List<String> selectedOptions = List.filled(7, '');

  final List<List<String>> questions = [
    [
      'Km Driven Range',
      'less than 50000',
      'less than 200000',
      'less than 400000',
      'less than 1000000'
    ],
    [
      'Price Range',
      'less than 40000',
      'less than 100000',
      'less than 300000',
      'less than 1000000',
      'less than 5000000',
      'less than 10000000'
    ],
    [
      'Year Range',
      'less than 2000',
      'less than 2010',
      'less than 2015',
      'less than 2024'
    ],
    ['Fuel Type', 'Petrol', 'Diesel', 'CNG'],
    ['Transmission', 'Manual', 'Automatic'],
    ['Owner', 'First', 'Second', 'Third', 'More'],
    ['Max Count', '1', '5', '10', '20'],
  ];

  void _nextQuestion() {
    setState(() {
      if (selectedOptions[currentQuestionIndex] == '') {
        return;
      }
      if (currentQuestionIndex < questions.length - 1) {
        currentQuestionIndex++;
      } else {
        // All questions answered, show recommendation screen
        Navigator.push(
          context,
          MaterialPageRoute(
            builder: (context) => CarRecommendationScreen(selectedOptions),
          ),
        );
      }
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Car Recommendation'),
      ),
      body: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        crossAxisAlignment: CrossAxisAlignment.center,
        children: [
          Text(
            questions[currentQuestionIndex][0],
            style: TextStyle(fontSize: 20, fontWeight: FontWeight.bold),
          ),
          SizedBox(height: 20),
          Column(
            children: List.generate(
              questions[currentQuestionIndex].length - 1,
              (index) => RadioListTile(
                title: Text(questions[currentQuestionIndex][index + 1]),
                value: questions[currentQuestionIndex][index + 1],
                groupValue: selectedOptions[currentQuestionIndex],
                onChanged: (value) {
                  setState(() {
                    selectedOptions[currentQuestionIndex] = value!;
                  });
                },
              ),
            ),
          ),
          SizedBox(height: 20),
          ElevatedButton(
            onPressed: _nextQuestion,
            child: Text('Next'),
          ),
        ],
      ),
    );
  }
}

class CarRecommendationScreen extends StatefulWidget {
  final List<String> selectedOptions;

  List<dynamic> resData = [];
  CarRecommendationScreen(this.selectedOptions);

  @override
  State<CarRecommendationScreen> createState() =>
      _CarRecommendationScreenState();
}

class _CarRecommendationScreenState extends State<CarRecommendationScreen> {
  _init() async {
    try {
      var kmDrivenRange = widget.selectedOptions[0].contains('50000')
          ? 0
          : widget.selectedOptions[0].contains('200000')
              ? 1
              : widget.selectedOptions[0].contains('400000')
                  ? 2
                  : 3;

      var priceRange = widget.selectedOptions[1].contains('40000')
          ? 0
          : widget.selectedOptions[1].contains('100000')
              ? 1
              : widget.selectedOptions[1].contains('300000')
                  ? 2
                  : widget.selectedOptions[1].contains('1000000')
                      ? 3
                      : widget.selectedOptions[1].contains('5000000')
                          ? 4
                          : 5;

      var yearRange = widget.selectedOptions[2].contains('2000')
          ? 0
          : widget.selectedOptions[2].contains('2010')
              ? 1
              : widget.selectedOptions[2].contains('2015')
                  ? 2
                  : 3;
      var fuelTypeId = widget.selectedOptions[3].contains('Petrol')
          ? 0
          : widget.selectedOptions[3].contains('2010')
              ? 1
              : 2;
      var transmissionId = widget.selectedOptions[4].contains('Manual') ? 0 : 1;
      var ownerTypeId = widget.selectedOptions[5].contains('First')
          ? 0
          : widget.selectedOptions[5].contains('Second')
              ? 1
              : widget.selectedOptions[5].contains('Third ')
                  ? 2
                  : 3;
      final response =
          await Dio().post('http://localhost:7184/api/Car/CarByParams', data: {
        'priceRange': priceRange,
        'yearRange': yearRange,
        'kmDrivenRange': kmDrivenRange,
        'fuelTypeId': fuelTypeId,
        'transmissionId': transmissionId,
        'ownerTypeId': ownerTypeId,
        'maxCountNum': widget.selectedOptions[6]
      });

      if (response.statusCode == 200) {
        final data = response.data as List<dynamic>;
        setState(() {
          widget.resData = data;
        });
      } else {
        throw Exception('Failed to fetch car data');
      }
    } catch (e) {
      throw Exception('Error: $e');
    }
  }

  @override
  void initState() {
    _init();
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Car Recommendations'),
      ),
      body: Container(
        child: widget.resData.isEmpty
            ? Container()
            : ListView.builder(
                itemCount: widget.resData.length,
                itemBuilder: (context, index) {
                  return Container(
                    margin: EdgeInsets.all(10),
                    decoration: BoxDecoration(
                        borderRadius: BorderRadius.all(Radius.circular(10)),
                        color: const Color.fromARGB(255, 251, 208, 81)),
                    child: Padding(
                      padding: const EdgeInsets.all(10),
                      child: Column(children: [
                        Text(
                          widget.resData[index]['name'].toString(),
                          style: TextStyle(
                              fontWeight: FontWeight.w900, fontSize: 18),
                        ),
                        Text('transmissions : ' +
                            widget.resData[index]['transmissions'].toString()),
                        Text('year : ' +
                            widget.resData[index]['year'].toString()),
                        Text('sellingPrice : ' +
                            widget.resData[index]['sellingPrice'].toString()),
                        Text('kmDriven : ' +
                            widget.resData[index]['kmDriven'].toString()),
                        Text('fuelType : ' +
                            widget.resData[index]['fuelType'].toString()),
                        Text('ownerType : ' +
                            widget.resData[index]['ownerType'].toString())
                      ]),
                    ),
                  );
                },
              ),
      ),
    );
  }
}
