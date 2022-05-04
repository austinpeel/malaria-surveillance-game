import './App.css';
import Header from './components/Header/Header';
import Footer from './components/Footer/Footer';
import UnityApp from './components/UnityApp/UnityApp';
import imagePlaceholder from './images/ClickToLoad.png';

const appData = {
  name: '',
  json: 'UnityGame/Build/UnityGame.json',
  unityLoader: 'UnityGame/Build/UnityLoader.js',
  description:
    'This is a game about the effectiveness of information regarding pregnant women in controlling malaria outbreaks.',
  image: imagePlaceholder,
};

function App() {
  return (
    <div className='container'>
      <Header />
      <UnityApp {...appData} />
      <Footer />
    </div>
  );
}

export default App;
