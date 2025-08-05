import React from "react";
import { View, Text, StyleSheet, TouchableOpacity } from "react-native";
import { TextInput } from "react-native-gesture-handler";

export default function Login({ navigation }: any) {
  return (
    <View style={styles.container}>
      <Text style={styles.title}>Login</Text>
      <Text style={styles.subtitle}>Faça login na sua conta</Text>

      <View style={styles.buttonsContainer}>
        <TouchableOpacity
          style={styles.buttonLogin}
          onPress={() => navigation.navigate('Home')}
        >
          <TextInput style={styles.buttonText}placeholder="Entrar"/>
        </TouchableOpacity>

        <TouchableOpacity
          style={[styles.buttonRegister, styles.buttonOutline]}
          onPress={() => navigation.navigate('Register')}
        >
          <Text style={[styles.buttonText, styles.buttonOutlineText]}>
            Não tenho uma conta
          </Text>
        </TouchableOpacity>
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#3B547B',
    alignItems: 'center',
    justifyContent: 'center',
    padding: 20,
  },
  title: {
    fontSize: 32,
    fontWeight: 'bold',
    color: '#FFFFFF',
    marginBottom: 10,
    fontFamily: 'Poppins',
  },
  subtitle: {
    fontSize: 16,
    textAlign: 'center',
    color: '#FFFFFF',
    marginBottom: 40,
  },
  buttonsContainer: {
    width: '100%',
  },
  buttonLogin: {
    backgroundColor: '#FF6F61',
    paddingVertical: 15,
    borderRadius: 5,
    alignItems: 'center',
    marginBottom: 10,
  },
  buttonRegister: {
    backgroundColor: '#FFFFFF',
    paddingVertical: 15,
    borderRadius: 5,
    alignItems: 'center',
  },
  buttonText: {
    color: '#FFFFFF',
    fontWeight: 'bold',
  },
  buttonOutlineText: {
    color: '#3B547B',
  },
    buttonOutline: {
        borderColor: '#3B547B',
        borderWidth: 1,
    },
});