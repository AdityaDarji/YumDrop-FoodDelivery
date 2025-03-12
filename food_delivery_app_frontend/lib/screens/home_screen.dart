import 'package:flutter/material.dart';
//import 'package:food_delivery_app_frontend/screens/login_screen.dart';
import 'package:shared_preferences/shared_preferences.dart';

class HomeScreen extends StatelessWidget {
  const HomeScreen({super.key});

  void _logout(BuildContext context) async {
    final prefs = await SharedPreferences.getInstance();
    await prefs.remove('token'); // Remove the stored token

    // Navigate back to login screen and remove all previous screens
    // ignore: use_build_context_synchronously
    Navigator.pushReplacementNamed(
      context,
      '/login',
    ); // Navigate to login screen
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text("Home"),
        actions: [
          IconButton(
            icon: Icon(Icons.logout),
            onPressed: () => _logout(context),
          ),
        ],
      ),

      body: const Center(
        child: Text(
          "Welcome to YumDrop!",
          style: TextStyle(fontSize: 20, fontWeight: FontWeight.bold),
        ),
      ),
    );
  }
}
