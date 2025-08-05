import React from 'react';
import { View, Text, StyleSheet, Image, TouchableOpacity } from 'react-native';

export default function WelcomeScreen({ navigation }: any) {
  return (
    <View style={styles.container}>
      <Image source={require('../../assets/Mooni.png')} style={styles.logo} />

      <Text style={styles.title}>Mooni</Text>
      <Text style={styles.subtitle}>
        O maior aliado para seu ciclo.
      </Text>

      <View style={styles.buttonsContainer}>
        <TouchableOpacity
          style={styles.buttonlogin}
          onPress={() => navigation.navigate('Login')}
        >
          <Text style={styles.buttonText}>Entrar</Text>
        </TouchableOpacity>

        <TouchableOpacity
          style={[styles.buttonRegister, styles.buttonOutline]}
          onPress={() => navigation.navigate('Register')}
        >
          <Text style={[styles.buttonText, styles.buttonOutlineText]}>Já possuo uma conta</Text>
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
  logo: {
    width: 200,
    height: 200,
    marginBottom: 20,
    resizeMode: 'contain',
  },
  title: {
    fontSize: 32,
    fontWeight: 'bold',
    color: '#FFff',
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
  buttonlogin: {
    backgroundColor: '#D9D9D9',
    padding: 15,
    borderRadius: 25,
    marginBottom: 15,
  },
  buttonRegister: {
    padding: 15,
    borderRadius: 25,
    marginBottom: 15,
  },

  buttonText: {
    color: '#3B547B',
    textAlign: 'center',
    fontWeight: '600',
  },
  buttonOutline: {
    borderColor: '#FF69B4',
  },
  buttonOutlineText: {
    color: '#ffffff',
    textDecorationLine: 'underline',
  },
});
