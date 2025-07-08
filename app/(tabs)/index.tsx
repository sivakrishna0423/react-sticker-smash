import Image from 'expo-image';
import { Link } from 'expo-router';
import { StyleSheet, Text, View } from 'react-native';
const placeHolderImage = require("@/assets/images/background-image.png");
export default function Index(){
  return(
<View style={styles.container}>
  <Text style={styles.text}>Home Screen</Text>
  <Link href="/about" style={styles.button}>
        Go to About screen
      </Link>
  <view style = {styles.imageContainer}>
    <Image source = {placeHolderImage} style={styles.image}/>
           
  </view>
</View>
  );
}

const styles=StyleSheet.create({
  container:{
    flex:1,
    backgroundColor:'#25292e',
    alignItems:'center',
    justifyContent:'center',
  },
  text:{
    color: '#fff',
  },
  button: {
    fontSize: 20,
    textDecorationLine: 'underline',
    color: '#fff',
  },
  imageContainer: {
    flex: 1
  },
  image:{
    width:320,
    height:440,
    borderRadius:18,
  },
});
