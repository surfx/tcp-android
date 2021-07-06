package br.com.controltcpandroid;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;

import android.content.Context;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.SeekBar;
import android.widget.TextView;

import com.google.android.material.slider.Slider;

import br.com.controltcpandroid.dialogs.DialogDesligar;
import br.com.controltcpandroid.tcpip.TcpClient;

public class MainActivity extends AppCompatActivity {

    private TcpClient client;
    private SeekBar mySeekBar;
    private TextView lblVolume, lblInformacoes;
    private EditText txtPorta, txtIp;
    private Button btnDesligar, btnSincronizar;
    private Context isto;
    private DialogDesligar dlg;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        client = new TcpClient();

        mySeekBar = findViewById(R.id.mySeekBar);
        lblVolume = findViewById(R.id.txtVolume);
        lblInformacoes = findViewById(R.id.lblInformacoes);
        btnDesligar = findViewById(R.id.btnDesligar);
        btnSincronizar = findViewById(R.id.btnSincronizar);
        txtPorta = findViewById(R.id.txtPorta);
        txtIp = findViewById(R.id.txtIp);

        isto = this;
        dlg = new DialogDesligar();

        mySeekBar.setMax(100);
        mySeekBar.setOnSeekBarChangeListener(new SeekBar.OnSeekBarChangeListener() {
            @Override
            public void onStopTrackingTouch(SeekBar seekBar) {
                lblVolume.setText("Volume End: " + seekBar.getProgress());
                int volume = seekBar.getProgress();
                alterarVolume(seekBar.getProgress());
            }

            @Override
            public void onStartTrackingTouch(SeekBar seekBar) {  }

            @Override
            public void onProgressChanged(SeekBar seekBar, int progress, boolean fromUser) {
                lblVolume.setText("Volume: " + progress);
            }
        });

        btnDesligar.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                lblInformacoes.setText("Desligar");
                dlg.showDialogDesligar(
                "Deseja desligar o computador ?", () -> {
                    //lblInformacoes.setText("Sim, desligar");
                    desligar();
                }, () ->{
                    //lblInformacoes.setText("Não, desligar");
                }, isto);
           }
        });

        btnSincronizar.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                lblInformacoes.setText("Sincronizar");
                sincronizar();
            }
        });

        sincronizar();
    }

    private int getPort(){
        int rt = 9876;
        try { rt = Integer.parseInt(txtPorta.getText().toString()); } catch (Exception e){  rt = 9876;  }
        return rt;
    }

    private String getIp(){
        String ip = "192.168.0.14";
        ip = txtIp.getText().toString();
        if (ip==null||ip.isEmpty()||ip.length()<=0){ ip = "192.168.0.14"; }
        return ip;
    }

    private void sincronizar(){
        int porta = getPort();
        String ip = getIp();

        // 0 - sincronizar
        client.enviarMensagem( ip, porta, "0", mensagem -> {
            runOnUiThread(() ->{
                if (mensagem==null||mensagem.isEmpty()|| !mensagem.substring(0,1).equals("1")){
                    lblInformacoes.setText("Erro ao sincronizar");
                    return;
                }
                lblInformacoes.setText("mensagem: " + mensagem);
                String volume = mensagem.substring(1);

                Float volumeFloat = -1.0F;
                try { volumeFloat = Float.parseFloat(volume); } catch (Exception e){ return; }
                if (volumeFloat < 0){ return; } //erro
                volumeFloat *= 100.0F;

                Integer volumeInteger = -1;
                try { volumeInteger = Math.round(volumeFloat); } catch (Exception e){ return; }
                if (volumeInteger < 0){ return; } //erro

                mySeekBar.setProgress(volumeInteger);
                lblInformacoes.setText("volume: " + volumeInteger);
            });
        });
    }

    private void alterarVolume(int volume){
        int porta = getPort();
        String ip = getIp();
        float volumeFloat = Float.parseFloat(volume + "") / 100.0F;

        // 1 - alterar volume
        client.enviarMensagem( ip, porta, "1" + volumeFloat, mensagem -> {
            runOnUiThread(()->{
                if (mensagem==null||mensagem.isEmpty()|| !mensagem.substring(0,1).equals("1")){
                    lblInformacoes.setText("Erro ao alterar o volume");
                    return;
                }
                lblInformacoes.setText("mensagem: " + mensagem);
            });
        });
    }

    private void desligar(){
        int porta = getPort();
        String ip = getIp();

        // 2 - desligar
        client.enviarMensagem( ip, porta, "2", mensagem -> {
            runOnUiThread(() ->{
                if (mensagem==null||mensagem.isEmpty()|| !mensagem.substring(0,1).equals("1")){
                    lblInformacoes.setText("Erro ao pedir para desligar");
                    return;
                }
                lblInformacoes.setText("mensagem: " + mensagem);
            });
        });
    }

}