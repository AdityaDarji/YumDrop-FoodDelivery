import 'package:flutter/material.dart';
import 'package:food_delivery_app_frontend/screens/login_screen.dart';
import 'package:food_delivery_app_frontend/screens/splash_screen.dart';
import 'package:food_delivery_app_frontend/screens/home_screen.dart';

void main() {
  runApp(MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      debugShowCheckedModeBanner: false,
      title: 'YumDrop',
      initialRoute: '/', // Set splash screen as initial route
      routes: {
        '/': (context) => const SplashScreen(),
        '/login': (context) => const LoginScreen(),
        '/home': (context) => const HomeScreen(),
      },
    );
  }
}
