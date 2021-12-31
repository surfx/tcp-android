package br.com.controltcpandroid.arquivos;

import android.content.Context;

import java.io.BufferedReader;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;

public class Arquivos {

    private Context fileContext;
    private String fileName = "arquivo.txt";

    public Arquivos(Context fileContext) { this.fileContext = fileContext; }

    public void salvar(String data) {
        try {
            FileOutputStream fOut = fileContext.openFileOutput(this.fileName, Context.MODE_PRIVATE);
            OutputStreamWriter osw = new OutputStreamWriter(fOut);
            osw.write(data);
            osw.flush();
            osw.close();
            System.out.println("ok");
        } catch (Exception e) {
            e.printStackTrace();
            System.out.println("erro");
        }
    }

    public String ler() {
        StringBuffer datax = new StringBuffer("");
        try {
            FileInputStream fIn = fileContext.openFileInput(this.fileName);
            InputStreamReader isr = new InputStreamReader(fIn);
            BufferedReader buffreader = new BufferedReader(isr);

            String readString = buffreader.readLine();
            while (readString != null) {
                datax.append(readString);
                readString = buffreader.readLine();
            }

            isr.close();
        } catch (IOException ioe) {
            System.out.println("erro");
            ioe.printStackTrace();
        }
        return datax.toString();
    }

}