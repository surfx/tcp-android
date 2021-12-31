package br.com.controltcpandroid;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.SeekBar;
import android.widget.TextView;

import java.util.Timer;
import java.util.TimerTask;

import br.com.controltcpandroid.arquivos.Arquivos;
import br.com.controltcpandroid.dialogs.DialogSimNao;
import br.com.controltcpandroid.tcpip.TcpClient;

public class MainActivity extends AppCompatActivity {

    private TcpClient client;
    private SeekBar mySeekBar;
    private TextView lblVolume, lblInformacoes;
    private EditText txtPorta, txtIp;
    private Button btnDesligar, btnSincronizar, btnMouseView, btnSalvar, btnLoad, btnResetData, btnTimer;
    private Context isto;
    private DialogSimNao dlg;
    private Arquivos arquivos;

    private boolean timer40minutosAtivo = false;
    private Timer timer;

    private final int portaPadrao = 9876;
    private final String ipPadrao = "192.168.0.14";

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
        btnMouseView = findViewById(R.id.btnMouseView);
        btnSalvar = findViewById(R.id.btnSalvar);
        btnLoad = findViewById(R.id.btnLoad);
        btnResetData = findViewById(R.id.btnResetData);
        btnTimer = findViewById(R.id.btnTimer);
        txtPorta = findViewById(R.id.txtPorta);
        txtIp = findViewById(R.id.txtIp);

        arquivos = new Arquivos(this);
        timer = new Timer();

        isto = this;
        dlg = new DialogSimNao();

        mySeekBar.setMax(100);
        mySeekBar.setOnSeekBarChangeListener(new SeekBar.OnSeekBarChangeListener() {
            @Override
            public void onStopTrackingTouch(SeekBar seekBar) {
                lblVolume.setText("Volume End: " + seekBar.getProgress());
                int volume = seekBar.getProgress();
                alterarVolume(seekBar.getProgress());
            }

            @Override
            public void onStartTrackingTouch(SeekBar seekBar) {
            }

            @Override
            public void onProgressChanged(SeekBar seekBar, int progress, boolean fromUser) {
                lblVolume.setText("Volume: " + progress);
            }
        });

        btnDesligar.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                lblInformacoes.setText("Desligar");
                dlg.showDialogSimNao(
                        "Deseja desligar o computador ?", () -> {
                            //lblInformacoes.setText("Sim, desligar");
                            desligar();
                        }, () -> {
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

        btnMouseView.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                lblInformacoes.setText("View Mouse");
                Intent intent = new Intent(isto, MouseActivity.class);
                intent.putExtra("porta", getPort());
                intent.putExtra("ip", getIp());
                startActivity(intent);
            }
        });

        btnSalvar.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                lblInformacoes.setText("Salvar");
                dlg.showDialogSimNao(
                        "Deseja salvar os dados ?", () -> {
                            saveData(getPort(), getIp());
                        }, () -> {

                        }, isto);
            }
        });

        btnLoad.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                lblInformacoes.setText("Load");
                dlg.showDialogSimNao(
                        "Deseja carregar os dados ?", () -> {
                            loadData();
                        }, () -> {

                        }, isto);
            }
        });

        btnResetData.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                lblInformacoes.setText("Load");
                dlg.showDialogSimNao(
                        "Deseja resetar os dados ?", () -> {
                            setPorta("" + portaPadrao);
                            setIp(ipPadrao);
                            saveData(getPort(), getIp());
                        }, () -> {

                        }, isto);
            }
        });

        btnTimer.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                lblInformacoes.setText("Timer");

                final long quarentaMinutos = 2400000L; // 40 min em milissegundos
                //final long quarentaMinutos = 2000L; // 2 segundos

                dlg.showDialogSimNao(
                        "Deseja desligar o PC em 40 minutos ?", () -> {
                            cancelTimer();

                            timer40minutosAtivo = true;
                            lblInformacoes.setText("Desligar em 40 minutos");
                            timer.schedule(new TimerTask() {
                                @Override
                                public void run() {
                                    if (!timer40minutosAtivo) { return; }
                                    desligar();
                                    runOnUiThread(new Runnable() {
                                        @Override
                                        public void run() {
                                            lblInformacoes.setText("Send command to shutdown...");
                                        }
                                    });
                                }
                            }, quarentaMinutos);
                        }, () -> {
                            cancelTimer();
                            lblInformacoes.setText("Timer Cancelado");
                        }, isto);
            }
        });

        loadData();
        sincronizar();
    }

    private int getPort() {
        int rt = portaPadrao;
        try { rt = Integer.parseInt(txtPorta.getText().toString()); } catch (Exception e) { rt = portaPadrao; }
        return rt;
    }

    private void setPorta(String porta) {
        if (porta == null || porta.isEmpty()) { porta = "" + portaPadrao; }
        txtPorta.setText(porta);
    }

    private String getIp() {
        String ip = ipPadrao;
        ip = txtIp.getText().toString();
        if (ip == null || ip.isEmpty() || ip.length() <= 0) { ip = ipPadrao; }
        return ip;
    }

    private void setIp(String ip) {
        if (ip == null || ip.isEmpty() || ip.length() <= 0) { ip = ipPadrao; }
        txtIp.setText(ip);
    }

    private void cancelTimer() {
        timer40minutosAtivo = false;
        if (timer != null) {
            try { timer.cancel(); timer.purge(); } catch (Exception e) {  }
            timer = null;
        }
        timer = new Timer();
    }

    private void saveData(int porta, String ip) {
        arquivos.salvar(ip + ";" + porta);
    }

    private void loadData() {
        String valor = arquivos.ler();
        if (valor == null || valor.isEmpty() || !valor.contains(";")) { return; }
        String[] vet = valor.split(";");
        if (vet == null || vet.length <= 1) { return; }
        String ip = vet[0];
        String porta = vet[1];

        setPorta(porta);
        setIp(ip);
    }

    private void sincronizar() {
        int porta = getPort();
        String ip = getIp();

        // 0 - sincronizar
        client.enviarMensagem(ip, porta, "0", mensagem -> {
            runOnUiThread(() -> {
                if (mensagem == null || mensagem.isEmpty() || !mensagem.substring(0, 1).equals("1")) { lblInformacoes.setText("Erro ao sincronizar"); return; }
                lblInformacoes.setText("mensagem: " + mensagem);
                String volume = mensagem.substring(1);

                Float volumeFloat = -1.0F;
                try { volumeFloat = Float.parseFloat(volume); } catch (Exception e) { return; }
                if (volumeFloat < 0) { return; } //erro
                volumeFloat *= 100.0F;

                Integer volumeInteger = -1;
                try { volumeInteger = Math.round(volumeFloat); } catch (Exception e) { return; }
                if (volumeInteger < 0) { return; } //erro

                mySeekBar.setProgress(volumeInteger);
                lblInformacoes.setText("volume: " + volumeInteger);
            });
        });
    }

    private void alterarVolume(int volume) {
        int porta = getPort();
        String ip = getIp();
        float volumeFloat = Float.parseFloat(volume + "") / 100.0F;

        // 1 - alterar volume
        client.enviarMensagem(ip, porta, "1" + volumeFloat, mensagem -> {
            runOnUiThread(() -> {
                if (mensagem == null || mensagem.isEmpty() || !mensagem.substring(0, 1).equals("1")) {
                    lblInformacoes.setText("Erro ao alterar o volume");
                    return;
                }
                lblInformacoes.setText("mensagem: " + mensagem);
            });
        });
    }

    private void desligar() {
        int porta = getPort();
        String ip = getIp();

        // 2 - desligar
        client.enviarMensagem(ip, porta, "2", mensagem -> {
            runOnUiThread(() -> {
                if (mensagem == null || mensagem.isEmpty() || !mensagem.substring(0, 1).equals("1")) {
                    lblInformacoes.setText("Erro ao pedir para desligar");
                    return;
                }
                lblInformacoes.setText("mensagem: " + mensagem);
            });
        });
    }

}