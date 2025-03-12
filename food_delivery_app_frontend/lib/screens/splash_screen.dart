import 'package:flutter/material.dart';
import 'package:flutter_animate/flutter_animate.dart';
import 'package:shared_preferences/shared_preferences.dart';

import 'package:food_delivery_app_frontend/core/constants.dart';

class SplashScreen extends StatefulWidget {
  const SplashScreen({super.key});

  @override
  // ignore: library_private_types_in_public_api
  _SplashScreenState createState() => _SplashScreenState();
}

class _SplashScreenState extends State<SplashScreen> {
  @override
  void initState() {
    super.initState();
    _checkLoginStatus();
  }
  /*Future.delayed(Duration(seconds: 3), () {
      Navigator.pushReplacement(
        // ignore: use_build_context_synchronously
        context,
        MaterialPageRoute(builder: (context) => LoginScreen()),
      );
    });*/

  Future<void> _checkLoginStatus() async {
    await Future.delayed(const Duration(seconds: 2)); // Simulate splash delay
    final prefs = await SharedPreferences.getInstance();
    final token = prefs.getString('token'); // Check if token exists

    if (!mounted) return; // Prevent context issue

    if (token != null && token.isNotEmpty) {
      Navigator.pushReplacementNamed(context, '/home'); // Navigate to home
    } else {
      Navigator.pushReplacementNamed(context, '/login'); // Navigate to login
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.primaryColor, // Use from constants
      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Image.asset(
                  'assets/images/logo.jpeg', // Make sure the logo exists here
                  width: 200, // Start small
                  height: 200,
                )
                .animate()
                .fade(duration: 1200.ms)
                .scale(
                  begin: const Offset(0.5, 0.5),
                  end: const Offset(1.2, 1.2),
                  duration: 1000.ms,
                )
                .then()
                .scale(
                  end: const Offset(1.0, 1.0),
                  duration: 800.ms,
                ), // Settle to final size

            SizedBox(height: 30),

            CircularProgressIndicator(
              color: AppColors.accentColor,
            ).animate().fade(duration: 800.ms, delay: 1.seconds),
          ],
        ),
      ),
    );
  }
}
