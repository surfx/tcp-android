package br.com.controltcpandroid;

import androidx.appcompat.app.AppCompatActivity;

import android.annotation.SuppressLint;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.SeekBar;
import android.widget.TextView;

import com.example.tcpandroid.R;

import java.util.Calendar;
import java.util.Timer;
import java.util.TimerTask;

import br.com.controltcpandroid.arquivos.Arquivos;
import br.com.controltcpandroid.dialogs.DialogSimNao;
import br.com.controltcpandroid.tcpip.binary.TCPClientBinary;
import br.com.controltcpandroid.util.BinaryUtil;
import br.com.controltcpandroid.util.MyBitSet;
import br.com.controltcpandroid.util.RespostaServidor;
import br.com.controltcpandroid.util.TCPUtil;

public class MainActivity extends AppCompatActivity {

    private SeekBar mySeekBar;
    private TextView lblVolume, lblInformacoes;
    private EditText txtPorta, txtIp;
    private Context isto;
    private DialogSimNao dlg;
    private Arquivos arquivos;

    private boolean timer40minutosAtivo = false;
    private Timer timer;

    private final int portaPadrao = 9876;
    private final String ipPadrao = "192.168.0.4";

    private TCPClientBinary getClient(){
        return new TCPClientBinary(getIp(), getPort());
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        mySeekBar = findViewById(R.id.mySeekBar);
        lblVolume = findViewById(R.id.txtVolume);
        lblInformacoes = findViewById(R.id.lblInformacoes);
        ImageButton btnDesligar = findViewById(R.id.btnDesligar);
        ImageButton btnSalvar = findViewById(R.id.btnSalvar);
        ImageButton btnSincronizar = findViewById(R.id.btnSincronizar);
        ImageButton btnMouseView = findViewById(R.id.btnMouseView);
        ImageButton btnLoad = findViewById(R.id.btnLoad);
        ImageButton btnResetData = findViewById(R.id.btnResetData);
        ImageButton btnTimer = findViewById(R.id.btnTimer);
        ImageButton btnLock = findViewById(R.id.btnLock);
        ImageButton btnUp = findViewById(R.id.btnUp);
        ImageButton btnDown = findViewById(R.id.btnDown);
        ImageButton btnLeft = findViewById(R.id.btnLeft);
        ImageButton btnRight = findViewById(R.id.btnRight);
        ImageButton btnClickMouseMain = findViewById(R.id.btnClickMouseMain);

        txtPorta = findViewById(R.id.txtPorta);
        txtIp = findViewById(R.id.txtIp);

        arquivos = new Arquivos(this);
        timer = new Timer();

        isto = this;
        dlg = new DialogSimNao();

        lblInformacoes.setText("");

        mySeekBar.setMax(100);
        mySeekBar.setOnSeekBarChangeListener(new SeekBar.OnSeekBarChangeListener() {
            @SuppressWarnings("unused")
            @SuppressLint("SetTextI18n")
            @Override
            public void onStopTrackingTouch(SeekBar seekBar) {
                lblVolume.setText("Volume: " + seekBar.getProgress());
                int volume = seekBar.getProgress();
                alterarVolume(seekBar.getProgress());
            }

            @Override
            public void onStartTrackingTouch(SeekBar seekBar) {
            }

            @SuppressLint("SetTextI18n")
            @Override
            public void onProgressChanged(SeekBar seekBar, int progress, boolean fromUser) {
                lblVolume.setText("Volume: " + progress);
            }
        });

        btnDesligar.setOnClickListener(v -> {
            lblInformacoes.setText("Desligar");
            //lblInformacoes.setText("Sim, desligar");
            dlg.showDialogSimNao(
                    "Deseja desligar o computador ?", this::desligar, () -> {
                        //lblInformacoes.setText("Não, desligar");
                    }, isto);
        });

        btnSincronizar.setOnClickListener(v -> {
            lblInformacoes.setText("Sincronizar");
            sincronizar();
        });

        btnMouseView.setOnClickListener(v -> {
            lblInformacoes.setText("View Mouse");
            Intent intent = new Intent(isto, MouseActivity.class);
            intent.putExtra("porta", getPort());
            intent.putExtra("ip", getIp());
            startActivity(intent);
        });

        btnSalvar.setOnClickListener(v -> {
            lblInformacoes.setText("Salvar");
            dlg.showDialogSimNao(
                    "Deseja salvar os dados ?", () -> saveData(getPort(), getIp()), () -> {

                    }, isto);
        });

        btnLoad.setOnClickListener(v -> {
            lblInformacoes.setText("Load");
            dlg.showDialogSimNao(
                    "Deseja carregar os dados ?", this::loadData, () -> {

                    }, isto);
        });

        btnResetData.setOnClickListener(v -> {
            lblInformacoes.setText("Load");
            dlg.showDialogSimNao(
                    "Deseja resetar os dados ?", () -> {
                        setPorta("" + portaPadrao);
                        setIp(ipPadrao);
                        saveData(getPort(), getIp());
                    }, () -> {

                    }, isto);
        });

        btnTimer.setOnClickListener(v -> {
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
                                runOnUiThread(() -> lblInformacoes.setText("Send command to shutdown..."));
                            }
                        }, quarentaMinutos);
                    }, () -> {
                        cancelTimer();
                        lblInformacoes.setText("Timer Cancelado");
                    }, isto);
        });

        btnLock.setOnClickListener(v -> {
            lblInformacoes.setText("Lock");

            lockScreen();
//            dlg.showDialogSimNao(
//                "Deseja bloquear a tela do PC ?", () -> {
//                    lockScreen();
//                }, () -> {
//                    lblInformacoes.setText("Lock Cancelado");
//                }, isto);
        });

        btnUp.setOnClickListener(v->{
            mouseUp();
        });

//        btnUp.setOnLongClickListener(new View.OnLongClickListener() {
//            @Override
//            public boolean onLongClick(View v) {
//                mouseUp();
//                return true;
//            }
//        });

        btnDown.setOnClickListener(v->{
            mouseDown();
        });

        btnLeft.setOnClickListener(v->{
            mouseLeft();
        });

        btnRight.setOnClickListener(v->{
            mouseRight();
        });

        btnClickMouseMain.setOnClickListener(v->{
            clickMouse();
        });

        loadData();
        sincronizar();
    }

    private int getPort() {
        @SuppressWarnings("UnusedAssignment") int rt = portaPadrao;
        try { rt = Integer.parseInt(txtPorta.getText().toString()); } catch (Exception e) { //noinspection ConstantConditions
            rt = portaPadrao; }
        return rt;
    }

    private void setPorta(String porta) {
        if (porta == null || porta.isEmpty()) { porta = "" + portaPadrao; }
        txtPorta.setText(porta);
    }

    private String getIp() {
        @SuppressWarnings("UnusedAssignment") String ip = ipPadrao;
        ip = txtIp.getText().toString();
        //noinspection ConstantConditions
        if (ip == null || ip.isEmpty() || ip.length() <= 0) { ip = ipPadrao; }
        return ip;
    }

    private void setIp(String ip) {
        //noinspection ConstantConditions
        if (ip == null || ip.isEmpty() || ip.length() <= 0) { ip = ipPadrao; }
        txtIp.setText(ip);
    }

    @SuppressWarnings("CatchMayIgnoreException")
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

    @SuppressWarnings("ConditionCoveredByFurtherCondition")
    private void loadData() {
        String valor = arquivos.ler();
        if (valor == null || valor.isEmpty() || !valor.contains(";")) { return; }
        String[] vet = valor.split(";");
        //noinspection ConstantConditions
        if (vet == null || vet.length <= 1) { return; }
        String ip = vet[0];
        String porta = vet[1];

        setPorta(porta);
        setIp(ip);
    }

    @SuppressWarnings("UnusedAssignment")
    @SuppressLint("SetTextI18n")
    private void sincronizar() {
        // 0 - sincronizar
        getClient().send(BinaryUtil.toMBitByte((byte)0, 4, false), retorno -> runOnUiThread(() -> {
            boolean bit0 = retorno.get(0);
            System.out.println("bit0:\t\t" + (bit0?1:0));

            MyBitSet volumeRetornoParser = retorno.slice(1, 32);
            System.out.println(volumeRetornoParser.toString() + ", valor: " + BinaryUtil.byteArrayToFloat(volumeRetornoParser.toByte(), false));

            float volumeFloat = BinaryUtil.byteArrayToFloat(volumeRetornoParser.toByte(), false);
            int volumeInteger = -1;
            try { volumeInteger = Math.round(volumeFloat); } catch (Exception e) { return; }
            if (volumeInteger < 0) { return; } //erro

            mySeekBar.setProgress(volumeInteger);
            lblInformacoes.setText("volume: " + volumeInteger);
        }));

//        getClient().enviarMensagem(ip, porta, "0", mensagem -> runOnUiThread(() -> {
//            //noinspection StringOperationCanBeSimplified
//            if (mensagem == null || mensagem.isEmpty() || !mensagem.substring(0, 1).equals("1")) { lblInformacoes.setText("Erro ao sincronizar"); return; }
//            lblInformacoes.setText("mensagem: " + mensagem);
//            String volume = mensagem.substring(1);
//
//            float volumeFloat = -1.0F;
//            try { volumeFloat = Float.parseFloat(volume); } catch (Exception e) { return; }
//            if (volumeFloat < 0) { return; } //erro
//            volumeFloat *= 100.0F;
//
//            int volumeInteger = -1;
//            try { volumeInteger = Math.round(volumeFloat); } catch (Exception e) { return; }
//            if (volumeInteger < 0) { return; } //erro
//
//            mySeekBar.setProgress(volumeInteger);
//            lblInformacoes.setText("volume: " + volumeInteger);
//        }));
    }

    @SuppressLint("SetTextI18n")
    private void alterarVolume(int volume) {
        float volumeFloat = Float.parseFloat(volume + "");

        MyBitSet entrada = BinaryUtil.toMBitByte((byte)1, 4, false);
        entrada.append(volumeFloat); // float - 4 bytes = 32 bits

        // 1 - alterar volume
        getClient().send(entrada, retorno -> runOnUiThread(() ->{
            boolean bit0 = retorno.get(0);
            System.out.println("bit0:\t\t" + (bit0?1:0));

            MyBitSet volumeRetornoParser = retorno.slice(1, 32);
            System.out.println(volumeRetornoParser.toString() + ", valor: " + BinaryUtil.byteArrayToFloat(volumeRetornoParser.toByte(), false));

            lblInformacoes.setText("mensagem: " + BinaryUtil.byteArrayToFloat(volumeRetornoParser.toByte(), false));
        }));
//        client.enviarMensagem(ip, porta, "1" + volumeFloat, mensagem -> runOnUiThread(() -> {
//            //noinspection StringOperationCanBeSimplified
//            if (mensagem == null || mensagem.isEmpty() || !mensagem.substring(0, 1).equals("1")) {
//                lblInformacoes.setText("Erro ao alterar o volume");
//                return;
//            }
//            lblInformacoes.setText("mensagem: " + mensagem);
//        }));
    }

    @SuppressLint("SetTextI18n")
    private void desligar() {
        // 2 - desligar
        getClient().send(BinaryUtil.toMBitByte((byte)2, 4, false), retorno -> runOnUiThread(() ->{
            RespostaServidor msg = TCPUtil.parserMensagemServer(retorno);
            System.out.println(msg.toString());

            lblInformacoes.setText("mensagem: " + msg.toString());
        }));
//        client.enviarMensagem(ip, porta, "2", mensagem -> runOnUiThread(() -> {
//            //noinspection StringOperationCanBeSimplified
//            if (mensagem == null || mensagem.isEmpty() || !mensagem.substring(0, 1).equals("1")) {
//                lblInformacoes.setText("Erro ao pedir para desligar");
//                return;
//            }
//            lblInformacoes.setText("mensagem: " + mensagem);
//        }));
    }

    private void lockScreen(){
        lblInformacoes.setText("lockScreen Method");

        getClient().send(BinaryUtil.toMBitByte((byte)5, 4, false), retorno -> runOnUiThread(() ->{
            RespostaServidor msg = TCPUtil.parserMensagemServer(retorno);
            System.out.println(msg.toString());

            lblInformacoes.setText("mensagem: " + msg.toString());
        }));

    }

    private void mouseUp(){
        lblInformacoes.setText("mouse up");

        // 6 - Up Mouse
        getClient().send(BinaryUtil.toMBitByte((byte)6, 4, false), retorno -> runOnUiThread(() ->{
            RespostaServidor msg = TCPUtil.parserMensagemServer(retorno);
            System.out.println(msg.toString());

            lblInformacoes.setText("mensagem: " + msg.toString());
        }));
    }

    private void mouseDown(){
        lblInformacoes.setText("mouse down");

        // 7 - Down Mouse
        getClient().send(BinaryUtil.toMBitByte((byte)7, 4, false), retorno -> runOnUiThread(() ->{
            RespostaServidor msg = TCPUtil.parserMensagemServer(retorno);
            System.out.println(msg.toString());

            lblInformacoes.setText("mensagem: " + msg.toString());
        }));
    }

    private void mouseLeft(){
        lblInformacoes.setText("mouse left");

        // 8 - Left Mouse
        getClient().send(BinaryUtil.toMBitByte((byte)8, 4, false), retorno -> runOnUiThread(() ->{
            RespostaServidor msg = TCPUtil.parserMensagemServer(retorno);
            System.out.println(msg.toString());

            lblInformacoes.setText("mensagem: " + msg.toString());
        }));
    }

    private void mouseRight(){
        lblInformacoes.setText("mouse right");

        // 9 - Right Mouse
        getClient().send(BinaryUtil.toMBitByte((byte)9, 4, false), retorno -> runOnUiThread(() ->{
            RespostaServidor msg = TCPUtil.parserMensagemServer(retorno);
            System.out.println(msg.toString());

            lblInformacoes.setText("mensagem: " + msg.toString());
        }));
    }

    @SuppressLint("SetTextI18n")
    private void clickMouse() {
        // 4 - click mouse
        getClient().send(BinaryUtil.toMBitByte((byte)4, 4, false), retorno -> runOnUiThread(() ->{
            RespostaServidor msg = TCPUtil.parserMensagemServer(retorno);
            System.out.println(msg.toString());

            lblInformacoes.setText("mensagem: " + msg.toString());
        }));
    }

}