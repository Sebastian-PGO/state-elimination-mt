package test;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.util.Random;

import org.eclipse.emf.common.util.EList;

import transitiongraph.State;
import transitiongraph.Transition;
import transitiongraph.TransitionGraph;

public class Tester extends TestFramework{
	
	private static final String NMF_EXE = "../NMFSolution/bin/Release/NMFSolution.exe";
	private long lastTime;
	
	public static void main(String[] args) {
		Tester test = new Tester();
		try {
			test.testFSAToRegexAllModels();
		} catch (FileNotFoundException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}
	
	public String FSAToRegex(TransitionGraph fsa, String path){
		
		try {
			File pathToExecutable = new File(NMF_EXE);
			ProcessBuilder processBuilder = new ProcessBuilder(pathToExecutable.getCanonicalPath());
			processBuilder.command().add(path);
			processBuilder.redirectErrorStream(true);
			Process process;
			process = processBuilder.start();
			InputStream stdout = process.getInputStream();
			BufferedReader reader = new BufferedReader(new InputStreamReader(stdout));
			String regex = reader.readLine();
			String time = reader.readLine();
			process.destroy();
			lastTime = Long.parseLong(time);
			return regex;
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (NumberFormatException e) {
			e.printStackTrace();
		}
		
		return "";
	}

	@Override
	public long getTime(long start, long end) {
		return lastTime;
	}
	
	public String DTMCToSRE(TransitionGraph dtmc, String path){
		return "";
	}
	
	public String getAcceptedWordFromAutomaton(TransitionGraph tg, int maxLength){
		String word = "";
		String longestAcceptingWord = null;
		EList<State> states = tg.getStates();
		EList<Transition> transitions;
		Random rand = new Random();
		Transition nextTransition;
		int count = 0;
		State currentState = null;
		
		for(State s: states){
			if(s.isIsInitial()){
				currentState = s;
				break;
			}
		}
		
		while(count < maxLength){
			transitions = currentState.getOutgoing();
			nextTransition = transitions.get(rand.nextInt(transitions.size()));
			word = word + nextTransition.getLabel();
			currentState = nextTransition.getTarget();
			
			if(currentState.isIsFinal()) longestAcceptingWord = word;
			
			count++;
		}
		
		return longestAcceptingWord;
	}
	
	public String getNotAcceptedWordFromAutomaton(TransitionGraph tg, int maxLength){
		String word = "";
		String longestNotAcceptingWord = null;
		EList<State> states = tg.getStates();
		EList<Transition> transitions;
		Random rand = new Random();
		Transition nextTransition;
		int count = 0;
		State currentState = null;
		
		for(State s: states){
			if(s.isIsInitial()){
				currentState = s;
				break;
			}
		}
		
		while(count < maxLength){
			transitions = currentState.getOutgoing();
			nextTransition = transitions.get(rand.nextInt(transitions.size()));
			word = word + nextTransition.getLabel();
			currentState = nextTransition.getTarget();
			
			if(!currentState.isIsFinal()) longestNotAcceptingWord = word;
			
			count++;
		}
		
		return longestNotAcceptingWord;
	}
	
}
