import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:flutter_secure_storage/flutter_secure_storage.dart';

class ApiService {
  static const String baseUrl = "http://192.168.31.224:5056/api/auth";

  static final _storage = FlutterSecureStorage(); // Secure token storage

  // User Registration API Call
  static Future<Map<String, dynamic>> registerUser(
    String username,
    String email,
    String phone,
    String password,
  ) async {
    final Uri url = Uri.parse('$baseUrl/register');

    final response = await http.post(
      url,
      headers: {"Content-Type": "application/json"},
      body: jsonEncode({
        "username": username,
        "email": email,
        "phone": phone,
        "password": password,
      }),
    );

    if (response.statusCode == 200) {
      return jsonDecode(response.body);
    } else {
      return {"error": jsonDecode(response.body)["message"]};
    }
  }

  static Future<Map<String, dynamic>> loginUser(
    String email,
    String password,
  ) async {
    try {
      final response = await http.post(
        Uri.parse("$baseUrl/login"),
        headers: {"Content-Type": "application/json"},
        body: jsonEncode({"email": email, "password": password}),
      );

      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);

        // Save JWT token securely
        await _storage.write(key: "token", value: data["token"]);

        return {"success": true, "token": data["token"]};
      } else {
        return {"error": "Invalid email or password"};
      }
    } catch (e) {
      return {"error": "Something went wrong. Try again later."};
    }
  }

  static Future<void> logoutUser() async {
    await _storage.delete(key: "token"); // Clear stored token
  }
}
