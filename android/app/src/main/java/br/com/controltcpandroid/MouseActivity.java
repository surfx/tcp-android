package br.com.controltcpandroid;

import android.annotation.SuppressLint;
import android.os.Bundle;
import android.util.DisplayMetrics;
import android.view.MotionEvent;
import android.view.View;
import android.widget.ImageButton;
import android.widget.TextView;

import androidx.appcompat.app.AppCompatActivity;

import com.example.tcpandroid.R;

import br.com.controltcpandroid.tcpip.binary.TCPClientBinary;
import br.com.controltcpandroid.util.BinaryUtil;
import br.com.controltcpandroid.util.MyBitSet;
import br.com.controltcpandroid.util.RespostaServidor;
import br.com.controltcpandroid.util.TCPUtil;

@SuppressWarnings("CommentedOutCode")
public class MouseActivity extends AppCompatActivity {

    private int porta;
    private String ip;
    private TextView txtInformacoes;
    private TCPClientBinary client;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_mouse);

        txtInformacoes = findViewById(R.id.txtInformacoes);
        ImageButton btnClickMouse = findViewById(R.id.btnClickMouse);

        getExtras();

        this.client = new TCPClientBinary(ip, porta);

        DisplayMetrics displayMetrics = new DisplayMetrics();
        getWindowManager().getDefaultDisplay().getMetrics(displayMetrics);
        int width = displayMetrics.widthPixels;
        int height = displayMetrics.heightPixels;

        View view = findViewById(R.id.activity_mouse);
        view.setOnTouchListener((v, event) -> onTouch(event, width, height));

        btnClickMouse.setOnClickListener(v -> {
            txtInformacoes.setText("click");
            clickMouse();
        });
    }

    @SuppressWarnings("StringOperationCanBeSimplified")
    @SuppressLint("SetTextI18n")
    private void clickMouse() {

        // 4 - click mouse
        client.send(BinaryUtil.toMBitByte((byte)4, 4, false), retorno -> runOnUiThread(() ->{
            RespostaServidor msg = TCPUtil.parserMensagemServer(retorno);
            System.out.println(msg.toString());

            txtInformacoes.setText("mensagem: " + msg.toString());
        }));
//        client.enviarMensagem(ip, porta, "4", mensagem -> runOnUiThread(() -> {
//            if (mensagem == null || mensagem.isEmpty() || !mensagem.substring(0, 1).equals("1")) {
//                txtInformacoes.setText("Erro ao enviar click");
//                return;
//            }
//            txtInformacoes.setText("mensagem: " + mensagem);
//        }));
    }

    @SuppressWarnings({"SameReturnValue", "StringOperationCanBeSimplified"})
    @SuppressLint("SetTextI18n")
    private boolean onTouch(MotionEvent event, int width, int height) {
        //System.out.println("------------------------------------------");
        //System.out.println("event: " + event);
        //System.out.println(event.getX() + ", " + event.getY() + " / ("+width+", "+height+")");

        // 3 - send mouse pos
        MyBitSet entrada = BinaryUtil.toMBitByte((byte)3, 4, false);
        entrada.append(BinaryUtil.toMBit(width, 13, false));
        entrada.append(BinaryUtil.toMBit(height, 13, false));
        entrada.append(BinaryUtil.toMBit((int) event.getX(), 13, false));
        entrada.append(BinaryUtil.toMBit((int) event.getY(), 13, false));

        client.send(entrada, retorno -> runOnUiThread(() ->{
            RespostaServidor msg = TCPUtil.parserMensagemServer(retorno);
            System.out.println(msg.toString());

            txtInformacoes.setText("mensagem: " + msg.toString());
        }));


//        client.enviarMensagem(ip, porta, mensagemSend, mensagem -> runOnUiThread(() -> {
//            if (mensagem == null || mensagem.isEmpty() || !mensagem.substring(0, 1).equals("1")) {
//                txtInformacoes.setText("Erro ao enviar informações do mouse");
//                return;
//            }
//            txtInformacoes.setText("mensagem: " + mensagem);
//        }));

        return true;
    }

    private void getExtras() {
        Bundle extras = getIntent().getExtras();
        if (extras == null) {
            return;
        }
        ip = extras.getString("ip");
        porta = extras.getInt("porta");
    }

}